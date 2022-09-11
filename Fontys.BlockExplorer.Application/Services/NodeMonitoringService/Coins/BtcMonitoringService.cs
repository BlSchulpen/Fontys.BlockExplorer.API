using AutoMapper;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService.Coins
{
    public class BtcMonitoringService : IBtcMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly IBtcNodeService _nodeService;
        private readonly IBtcBlockProvider _btcBlockProvider;
        private readonly CoinType _coinType;

        public BtcMonitoringService(BlockExplorerContext blockExplorerContext, IBtcNodeService nodeService, IBtcBlockProvider btcBlockProvider)
        {
            _context = blockExplorerContext;
            _btcBlockProvider = btcBlockProvider;
            _nodeService = nodeService; 
            _coinType =  CoinType.BTC;
        }

        public async Task<ICollection<Block>> RemoveBadBlocksAsync()
        {
            var removedBlocks = new List<Block>();
            if (!_context.Blocks.Any())
                return removedBlocks;

            var storedHeight = _context.Blocks.DefaultIfEmpty().Max(x => x.Height);
            var storedBlock = _context.Blocks.FirstOrDefault(b => b.Height == storedHeight);
            var chainHash = await _nodeService.GetHashFromHeightAsync(storedBlock.Height);
            while (storedBlock.Hash != chainHash)
            {
                _context.Blocks.Remove(storedBlock);
                removedBlocks.Add(storedBlock);
                storedHeight -= 1;
                storedBlock = _context.Blocks.FirstOrDefault(b => b.Height == storedHeight);
                chainHash = await _nodeService.GetHashFromHeightAsync(storedBlock.Height);
            }
            await _context.SaveChangesAsync();
            return removedBlocks;
        }

        public async Task<ICollection<Block>> GetNewBlocksAsync()
        {
            try
            {
                var newBlocks = new List<Block>();
                if (_context.Blocks.Where(b => b.CoinType == _coinType).ToList().Count == 0)
                {
                    var firstBlock = await StoreFirstBlockAsync();
                    newBlocks.Add(firstBlock);
                }
                var storedHeight = _context.Blocks.Where(b => b.CoinType == CoinType.BTC).Max(x => x.Height);
                var chainHash = await _nodeService.GetBestBlockHashAsync();
                var chainBlock = await GetBlockAsync(chainHash);
                var newHeight = chainBlock.Height;
                while (storedHeight < chainBlock.Height && _context.Blocks.Count() < newHeight)
                {
                    if (!_context.Blocks.Any(b => b.Height == chainBlock.Height))
                    {
                        _context.Add(chainBlock);
                        newBlocks.Add(chainBlock);
                        await _context.SaveChangesAsync();
                    }
                    chainHash = await _nodeService.GetHashFromHeightAsync(chainBlock.Height - 1);
                    chainBlock = chainBlock = await GetBlockAsync(chainHash);
                }
                return newBlocks;
            }
            catch (Exception e)
            {
                var test = 0;
                throw;
            }
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




            private async Task<int> InitialBlockHeight(ICollection<Block> newBlocks) //todo maybe add extensions to clean code?
        {
            if (_context.Blocks.ToList().Count == 0)
            {
                var initialHeight = 0;
                var initialHash = await _nodeService.GetHashFromHeightAsync(initialHeight);
                var initialBlock = await GetBlockAsync(initialHash);
                _context.Blocks.Add(initialBlock);
                await _context.SaveChangesAsync();
                newBlocks.Add(initialBlock);
                return initialHeight;
            }
            var storedHeight = _context.Blocks.Max(x => x.Height);
            return storedHeight;
        }


        private async Task UpdateAddressesAsync(Block block)  //todo test and maybe consider somehting else
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // select all addresses
            var addresses = new List<Address>();
            var inputAddresses = block.Transactions.Where(t => t.Inputs != null).SelectMany(tx => tx.Inputs).Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            var outputAddresses = block.Transactions.Where(t => t.Outputs != null).SelectMany(tx => tx.Outputs).Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            addresses.AddRange(inputAddresses);
            addresses.AddRange(outputAddresses);

            // make addresses unique
            var distinctAddresses = addresses
                .GroupBy(i => i.Hash)
                .Select(i => i.First())
                .ToList();
            var distinctAddressesHashes = distinctAddresses.Select(a => a.Hash).ToList();
            // context get address that aren't in the DB 
            var storedAddresses = _context.Addresses.Where(a => distinctAddressesHashes.Contains(a.Hash)).ToList();
            var storedAddressesHashes = storedAddresses.Select(a => a.Hash);
            //   List<Address> newAddresses = addresses.Where(a => !storedAddressesHashes.Contains(a.Hash)).Select(a => a.Hash).Distinct().ToList();
            var newAddresses = addresses.GroupBy(x => x.Hash).Select(y => y.First()).ToList();

            // Then add these addresses
            foreach (var a in newAddresses)
            {
                _context.Addresses.Add(a);
                await _context.SaveChangesAsync();
            }
            // then for all addresses just get context address

            //messure time and maybe change
            foreach (var input in block.Transactions.ToList().SelectMany(t => t.Inputs).Where(a => a.Address != null))
            {
                input.Address = _context.Addresses.FirstOrDefault(x => x.Hash == input.Address.Hash);
            }

            //    block.Transactions.ToList().ForEach(t => t.Inputs.ToList().ForEach(input => input.Address = _context.Addresses.FirstOrDefault(a => a.Hash == input.Address.Hash)));
            block.Transactions.ToList().ForEach(t => t.Outputs.ToList().ForEach(input => input.Address = _context.Addresses.FirstOrDefault(a => a.Hash == "00000000f119353e14aedcc858659bd270c4a4e33d4aa23f275e160e1b3c8f91")));

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var test = 1;
        }
    }
}
