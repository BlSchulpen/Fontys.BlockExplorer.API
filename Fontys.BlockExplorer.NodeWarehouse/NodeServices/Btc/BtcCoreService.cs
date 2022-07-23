namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.Extensions.Options;

    public class BtcCoreService : IExplorerBtcService
    {
        private readonly NodeOptions _options;

        public BtcCoreService(IOptions<NodeOptions> nodeOptions)
        {
            _options = nodeOptions.Value;
        }

        public Task<string> GetBestBlockHashAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Block> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }
    }
}
