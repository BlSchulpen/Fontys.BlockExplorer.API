using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class BtcBlockProviderService : IBlockProviderService
    {
        public Task<Block> GetBlockAsync(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
