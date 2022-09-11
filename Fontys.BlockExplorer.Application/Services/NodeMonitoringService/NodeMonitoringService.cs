using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    public class NodeMonitoringService : INodeMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly Func<CoinType, IBlockDataProviderService> _providerServiceResolver;

        public NodeMonitoringService(BlockExplorerContext blockExplorerContext, Func<CoinType, IBlockDataProviderService> providerServiceResolver)
        {
            _context = blockExplorerContext;
        }

        public async Task<ICollection<Block>> RemoveBadBlocksAsync(CoinType coinType)
        {
            IBlockDataProviderService providerService = _providerServiceResolver(coinType);
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
                chainHash = await _nodeService.GetHashFromHeightAsync(storedBlock.Height);
            }
            await _context.SaveChangesAsync();
            return removedBlocks;
        }

        public async Task<ICollection<Block>> GetNewBlocksAsync(CoinType coinType)
        {
            var newBlocks = new List<Block>();
            if (_context.Blocks.Where(b => b.CoinType == _coinType).ToList().Count == 0)
            {
                var firstBlock = await StoreFirstBlockAsync();
                newBlocks.Add(firstBlock);
            }

            var storedHeight = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Max(x => x.Height);
            var chainHash = await _nodeService.GetBestBlockHashAsync();
            var chainBlock = await _btcBlockProvider.GetBlockAsync(chainHash);
            var newHeight = chainBlock.Height;
            while (storedHeight < chainBlock.Height && _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Count() < newHeight)
            {
                if (!_context.Blocks.Any(b => b.Height == chainBlock.Height && chainBlock.CoinType == _coinType))
                {
                    _context.Add(chainBlock);
                    newBlocks.Add(chainBlock);
                    await _context.SaveChangesAsync();
                }
                chainHash = await _nodeService.GetHashFromHeightAsync(chainBlock.Height - 1);
                chainBlock = await _btcBlockProvider.GetBlockAsync(chainHash);
            }
            return newBlocks;

        }

        private async Task<Block> StoreFirstBlockAsync()
        {
            var firstBlockHeight = 0;
            var firstBlockHash = await _nodeService.GetHashFromHeightAsync(firstBlockHeight);
            var firstBlock = await _btcBlockProvider.GetBlockAsync(firstBlockHash);
            await UpdateAddressesAsync(firstBlock);
            _context.Blocks.Add(firstBlock);
            await _context.SaveChangesAsync();
            return firstBlock;
        }
    }
}
