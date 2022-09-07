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
            var tes = await _nodeService.GetBlockFromHashAsync("00000000839a8e6886ab5951d76f411475428afc90947ee320161bbf18eb6048");
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
            var chainBlock = await _nodeService.GetBlockFromHashAsync(chainHash);
            while (storedHeight < chainBlock.Height && _context.Blocks.Count() < chainBlock.Height)
            {
                if (!_context.Blocks.Any(b => b.Height == chainBlock.Height))
                {
                    _context.Add(chainBlock);
                    newBlocks.Add(chainBlock);
                    await _context.SaveChangesAsync();
                }
                chainHash = await _nodeService.GetHashFromHeightAsync(chainBlock.Height - 1);
                chainBlock = await _nodeService.GetBlockFromHashAsync(chainHash);
            }
            return newBlocks;
        }

        private async Task<int> InitialBlockHeight(ICollection<Block> newBlocks)
        {
            if (_context.Blocks.ToList().Count == 0)
            {
                var initialHeight = 0;
                var initialHash = await _nodeService.GetHashFromHeightAsync(initialHeight);
                var initialBlock = await _nodeService.GetBlockFromHashAsync(initialHash);
                _context.Blocks.Add(initialBlock);
                await _context.SaveChangesAsync();
                newBlocks.Add(initialBlock);
                return initialHeight;
            }
            var storedHeight = _context.Blocks.Max(x => x.Height);
            return storedHeight;
        }
    }
}
