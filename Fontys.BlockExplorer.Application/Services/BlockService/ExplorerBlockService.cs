using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    public class ExplorerBlockService : IBlockService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        
        public ExplorerBlockService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Block?> GetBlockAsync(GetBlockCommand command)
        {
            var hash = command.Hash;
            var stored = await _blockExplorerContext.Blocks
                .Include(b => b.Transactions)
                    .ThenInclude(t => t.Inputs)
                        .ThenInclude(i => i.Address)

                .Include(b => b.Transactions)
                    .ThenInclude(t => t.Outputs)
                        .ThenInclude(o => o.Address)
                .FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
