using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    public class ExplorerNodeMonitoringService : INodeMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly IBlockService _blockService;
        private readonly ILogger<ExplorerNodeMonitoringService> _logger;
        private readonly IBlockDataProviderService _providerService;

        //  private readonly Func<CoinType, IBlockDataProviderService> _providerServiceResolver;

        public ExplorerNodeMonitoringService(BlockExplorerContext blockExplorerContext, IBlockDataProviderService providerService, IBlockService blockService, ILogger<ExplorerNodeMonitoringService> logger)
        {
            _context = blockExplorerContext;
            _logger = logger;
            // _providerServiceResolver = providerServiceResolver;
            _blockService = blockService;
            _providerService = providerService;
        }

        public async Task<ICollection<Block>> RemoveBadBlocksAsync(CoinType coinType)
        {
            //   var providerService = _providerServiceResolver(coinType);
            var toRemoveBlocks = new List<Block>();
            if (!_context.Blocks.Any())
            {
                _logger.LogInformation("Couldn't find any bad blocks, no blocks are stored");
                return toRemoveBlocks;
            }

            var heightStoredBlock = _context.Blocks.DefaultIfEmpty().Max(x => x.Height);
            var storedBlock = _context.Blocks.FirstOrDefault(b => b.Height == heightStoredBlock);
            var hashBlockInBlockchain = await _providerService.GetHashFromHeightAsync(storedBlock.Height);
            while (storedBlock.Hash != hashBlockInBlockchain)
            {
                toRemoveBlocks.Add(storedBlock);
                heightStoredBlock -= 1;
                storedBlock = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).FirstOrDefault(b => b.Height == heightStoredBlock);
                hashBlockInBlockchain = await _providerService.GetHashFromHeightAsync(storedBlock.Height);
            }
            await _blockService.RemoveBlocksAsync(toRemoveBlocks);
            return toRemoveBlocks;
        }

        public async Task<ICollection<Block>> GetNewBlocksAsync(CoinType coinType)
        {
            //    var providerService = _providerServiceResolver(coinType);
            var newBlocks = await GetStartingBlockListAsync(coinType);
            var storedHeight = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Max(x => x.Height);
            var chainBlock = await GetBestBlockAsync(_providerService);
            if (chainBlock == null)
            {

                throw new NullReferenceException("No blocks can be found in the chain");
            }

            var latestHeight = chainBlock.Height;
            var nrStored = _context.Blocks.Count(b => b.CoinType == CoinType.BTC);
            while (storedHeight < chainBlock.Height || nrStored <= latestHeight)
            {
                if (!_context.Blocks.Any(b => b.Height == chainBlock.Height && chainBlock.CoinType == coinType))
                {
                    newBlocks.Add(chainBlock);
                    await _blockService.AddBlockAsync(chainBlock);
                    nrStored += 1;
                }

                if (chainBlock.Height == 0)
                    continue;
                var chainHash = await _providerService.GetHashFromHeightAsync(chainBlock.Height - 1);
                chainBlock = await _providerService.GetBlockAsync(chainHash);
            }

            return newBlocks;
        }

        private async Task<Block> GetBestBlockAsync(IBlockDataProviderService providerService)
        {
            var chainHash = await providerService.GetBestBlockHashAsync();
            var chainBlock = await providerService.GetBlockAsync(chainHash);
            return chainBlock;
        }

        private async Task<List<Block>> GetStartingBlockListAsync(CoinType coinType)
        {
            var blocks = new List<Block>();
            if (_context.Blocks.Where(b => b.CoinType == coinType).ToList().Count != 0)
            {
                return blocks;
            }
            var firstBlock = await GetFirstBlockAsync(coinType);
            await _blockService.AddBlockAsync(firstBlock);
            blocks.Add(firstBlock);
            return blocks;
        }

        private async Task<Block> GetFirstBlockAsync(CoinType coinType)
        {
            //     var providerService = _providerServiceResolver(coinType);
            var firstBlockHeight = 0;
            var firstBlockHash = await _providerService.GetHashFromHeightAsync(firstBlockHeight);
            var firstBlock = await _providerService.GetBlockAsync(firstBlockHash);
            return firstBlock;
        }
    }
}
