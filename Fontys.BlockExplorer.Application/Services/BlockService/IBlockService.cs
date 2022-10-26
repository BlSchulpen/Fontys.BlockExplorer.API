using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    public interface IBlockService
    {
        public Task<Block?> GetBlockAsync(string hash);
        public Task AddBlockAsync(Block block);
        public Task RemoveBlocksAsync(List<Block> blocks);
    }
}
