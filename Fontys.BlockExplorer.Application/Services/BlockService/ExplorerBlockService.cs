namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExplorerBlockService : IBlockService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        
        public ExplorerBlockService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Block> GetBlockAsync(GetBlockCommand getBlockCommand)
        {
            var hash = getBlockCommand.Hash;
            var storedHash = await _blockExplorerContext.Blocks.FirstOrDefaultAsync(b => b.Hash == hash);
            return storedHash;
        }
    }
}
