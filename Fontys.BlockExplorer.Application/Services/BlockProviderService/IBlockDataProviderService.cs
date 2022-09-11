using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public interface IBlockDataProviderService
    {
        Task<Block> GetBlockAsync(string hash);
        Task<string> GetHashFromHeightAsync(int height);
        Task<string> GetBestBlockHashAsync();
    }
}
