using AutoMapper;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;

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
            var newBlocks = new List<Block>();
            var storedHeight = await InitialBlockHeight(newBlocks);
            var chainHash = await _nodeService.GetBestBlockHashAsync();
            var chainBlockResponse = await _nodeService.GetBlockFromHashAsync(chainHash);
            var chainBlock = _mapper.Map<Block>(chainBlockResponse);
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
                chainBlockResponse = await _nodeService.GetBlockFromHashAsync(chainHash);
                chainBlock = _mapper.Map<Block>(chainBlockResponse);
                var test = _context.Blocks.Count(); 
            }
            return newBlocks;
        }

        private async Task<int> InitialBlockHeight(ICollection<Block> newBlocks)
        {
            if (_context.Blocks.ToList().Count == 0)
            {
                var initialHeight = 0;
                var initialHash = await _nodeService.GetHashFromHeightAsync(initialHeight);
                var initialBlockResponse = await _nodeService.GetBlockFromHashAsync(initialHash);
                var initialBlock = _mapper.Map<Block>(initialBlockResponse);
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
        private async Task<Block> GetBlock(string hash)
        {
            var blockResponse = await _nodeService.GetBlockFromHashAsync(hash);
            foreach (var transaction in blockResponse.Tx)
            {
                var usedIndexes = new List<int>();
                foreach (var input in transaction.Vin)
                {
                    var rawTransaction = await _nodeService.GetRawTransactionAsync(input.TxId);
                    var usedOutput = rawTransaction.Vout.FirstOrDefault(v => v.N == input.Vout); //inputs of this transaction are the outputs of another transaction
                    if (usedOutput != null)
                    {
                        input.Address = usedOutput.Address;
                        input.Value = usedOutput.Value;
                    }
                }
            }
            var block = _mapper.Map<Block>(blockResponse);
            return block;
        }
    }
}
