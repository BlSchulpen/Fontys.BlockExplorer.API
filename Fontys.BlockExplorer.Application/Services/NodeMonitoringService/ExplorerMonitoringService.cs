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

        public async Task<ICollection<Block>> UpdateStoredAsync()
        {
           await RemoveBadBlocksAsync();
           return await GetNewBlocksAsync();
        }

        private async Task<ICollection<Block>> RemoveBadBlocksAsync()
        {
            var badBlocks = new List<Block>();
            var storedHeight = _context.Blocks.Max(b => b.Height);
            var storedBlock = _context.Blocks.FirstOrDefault(b => b.Height == storedHeight);
            var nodeHash = await _nodeService.GetHashFromHeight(storedHeight);
            var nodeBlock = await _nodeService.GetBlockFromHashAsync(nodeHash);
            while (nodeBlock.Hash != storedBlock.Hash)
            {
                badBlocks.Add(storedBlock);
                nodeBlock = await _nodeService.GetBlockFromHashAsync(nodeBlock.PreviousHash);
                storedBlock = _context.Blocks.FirstOrDefault(b => b.Hash == storedBlock.PreviousHash);
            }
            return badBlocks;
        }

        private async Task<ICollection<Block>> GetNewBlocksAsync()
        {
            var newBlocks = new List<Block>();
            var nodeHash = await _nodeService.GetBestBlockHashAsync();
            var nodeBlock = await _nodeService.GetBlockFromHashAsync(nodeHash);
            while (!_context.Blocks.Any(b => b.Hash == nodeHash))
            {
                newBlocks.Add(nodeBlock);
                _context.Add(nodeBlock);
                nodeHash = nodeBlock.PreviousHash;
                nodeBlock = await _nodeService.GetBlockFromHashAsync(nodeHash);
            }
            await _context.SaveChangesAsync();
            return newBlocks;
        }

    }
}
