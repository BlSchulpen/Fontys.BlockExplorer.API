using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    public interface IBlockService
    {
        public List<Block> GetBlocks(CoinType coinType);
        public List<Block> GetLatestTenBlock(CoinType coinType);
        public Task<Block?> GetBlockAsync(string hash, CoinType coinType);
        public Task AddBlockAsync(Block block);
        public Task RemoveBlocksAsync(List<Block> blocks);
    }
}
