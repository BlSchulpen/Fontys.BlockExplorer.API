using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    public class NodeMonitoringService : INodeMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly IAddressRestoreService _addressRestoreService;
        private readonly Func<CoinType, IBlockDataProviderService> _providerServiceResolver;

        public NodeMonitoringService(BlockExplorerContext blockExplorerContext, Func<CoinType, IBlockDataProviderService> providerServiceResolver, IAddressRestoreService addressRestoreService)
        {
            _context = blockExplorerContext;
            _providerServiceResolver = providerServiceResolver;
            _addressRestoreService = addressRestoreService;
        }

        public async Task<ICollection<Block>> RemoveBadBlocksAsync(CoinType coinType)
        {
            var providerService = _providerServiceResolver(coinType);
            var removedBlocks = new List<Block>();
            if (!_context.Blocks.Any())
                return removedBlocks;

            var storedHeight = _context.Blocks.DefaultIfEmpty().Max(x => x.Height);
            var storedBlock = _context.Blocks.FirstOrDefault(b => b.Height == storedHeight);
            var chainHash = await providerService.GetHashFromHeightAsync(storedBlock.Height);
            while (storedBlock.Hash != chainHash)
            {
                _context.Blocks.Remove(storedBlock);
                removedBlocks.Add(storedBlock);
                storedHeight -= 1;
                storedBlock = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).FirstOrDefault(b => b.Height == storedHeight);
                chainHash = await providerService.GetHashFromHeightAsync(storedBlock.Height);
            }
            await _context.SaveChangesAsync();
            return removedBlocks;
        }

        public async Task<ICollection<Block>> GetNewBlocksAsync(CoinType coinType)
        {
            var providerService = _providerServiceResolver(coinType);
            var newBlocks = new List<Block>();
            if (_context.Blocks.Where(b => b.CoinType == coinType).ToList().Count == 0)
            {
                var firstBlock = await StoreFirstBlockAsync(coinType);
                newBlocks.Add(firstBlock);
            }

            var storedHeight = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Max(x => x.Height);
            var chainHash = await providerService.GetBestBlockHashAsync();
            var chainBlock = await providerService.GetBlockAsync(chainHash);
            var newHeight = chainBlock.Height;
            while (storedHeight < chainBlock.Height && _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Count() < newHeight)
            {
                if (!_context.Blocks.Any(b => b.Height == chainBlock.Height && chainBlock.CoinType == coinType))
                {
                    newBlocks.Add(chainBlock);
                    await StoreBlockAsync(chainBlock);
                }
                chainHash = await providerService.GetHashFromHeightAsync(chainBlock.Height - 1);
                chainBlock = await providerService.GetBlockAsync(chainHash);
            }
            return newBlocks;

        }

        private async Task<Block> StoreFirstBlockAsync(CoinType coinType)
        {
            var providerService = _providerServiceResolver(coinType);
            var firstBlockHeight = 0;
            var firstBlockHash = await providerService.GetHashFromHeightAsync(firstBlockHeight);
            var firstBlock = await providerService.GetBlockAsync(firstBlockHash);
            await StoreBlockAsync(firstBlock);
            return firstBlock;
        }

        private async Task StoreBlockAsync(Block block)
        {
            await _addressRestoreService.RestoreAddressesAsync(block);
            _context.Blocks.Add(block);
            await _context.SaveChangesAsync();
        }
    }
}
