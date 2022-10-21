using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    public interface IBlockService
    {
        public Task<Block?> GetBlockAsync(GetBlockCommand getBlockCommand);
        public Task AddBlockAsync(Block block);
    }
}
