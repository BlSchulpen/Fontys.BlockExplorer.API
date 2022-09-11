using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public interface IBlockProviderService
    {
        Task<Block> GetBlockAsync(string hash);
    }
}
