namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;

    public class ExplorerMonitoringService : INodeMonitoringService
    {
        private readonly BlockExplorerContext _context;
        private readonly INodeService _nodeService;

        public ExplorerMonitoringService(BlockExplorerContext blockExplorerContext, INodeService nodeService) //TODO coins to nodes
        {
            _context = blockExplorerContext;
            _nodeService = nodeService;
        }

        public async Task<ICollection<Block>> RemoveBadBlocksAsync()
        {
            var removedBlocks = new List<Block>();
            var storedHeight = _context.Blocks.Max(x => x.Height);
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
            var storedHeight = _context.Blocks.Max(x => x.Height);
            var chainHash = await _nodeService.GetBestBlockHashAsync();
            var chainBlock = await _nodeService.GetBlockFromHashAsync(chainHash);
            while (storedHeight < chainBlock.Height)
            {
                _context.Add(chainBlock);
                newBlocks.Add(chainBlock);
                chainHash = chainBlock.PreviousHash;
                chainBlock = await _nodeService.GetBlockFromHashAsync(chainHash);
            }
            await _context.SaveChangesAsync();
            return newBlocks;
        }
    }
}
