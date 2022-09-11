using AutoMapper;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using System.Linq;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    public class ExplorerMonitoringService : INodeMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly INodeService _nodeService;
        private readonly IMapper _mapper;

        public ExplorerMonitoringService(BlockExplorerContext blockExplorerContext, INodeService nodeService, IMapper mapper)
        {
            _context = blockExplorerContext;
            _nodeService = nodeService;
            _mapper = mapper;
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
                var storedHeight = await InitialBlockHeight(newBlocks);
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

        //todo this should be a lot easier to read
        //TODO IMPROVE CODE QUALITY
        //TODO UNIT TEST
        private async Task<Block> GetBlockAsync(string hash) //!!!! todo check if address excists...
        {
            hash = "0000000002fe34683160f929c9d68988e6173ec5dd8e78efd888dcaa5959707e";
            var blockResponse = await _nodeService.GetBlockFromHashAsync(hash);
            foreach (var transaction in blockResponse.Tx)
            {
                var usedIndexes = new List<int>();
                foreach (var input in transaction.Vin.Where(t => t.TxId != null))
                {
                    var rawTransaction = await _nodeService.GetRawTransactionAsync(input.TxId);
                    var usedOutput = rawTransaction.Vout.FirstOrDefault(v => v.N == input.Vout); //inputs of this transaction are the outputs of another transaction
                    if (usedOutput != null)
                    {
                        input.Addresses = usedOutput.ScriptPubKey.Addresses; //maybe try ti get this info from key instead to improve preformance...
                        input.Value = usedOutput.Value;
                    }
                }
            }
            var block = _mapper.Map<Block>(blockResponse);
            await UpdateAddressesAsync(block); //0000000000000000036b7443e223c73f5a075c9d18a957061664b0d7e18c7ae9
            return block;
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
//            var uniqueOutputAddresses = block.Transactions.Select(t => t.Outputs.Select(o => o.Address.Hash));
