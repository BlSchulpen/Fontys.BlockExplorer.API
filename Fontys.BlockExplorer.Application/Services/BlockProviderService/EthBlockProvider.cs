using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class EthBlockProvider : IBlockDataProviderService
    {
        public Task<Block> GetBlockAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBestBlockHashAsync()
        {
            throw new NotImplementedException();
        }
    }
}
