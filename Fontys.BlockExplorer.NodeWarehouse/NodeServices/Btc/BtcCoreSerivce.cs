namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices

{
    using Fontys.BlockExplorer.Domain.Models;

    public class BtcCoreSerivce : IExplorerBtcService
    {
        private readonly NodeOptions _options;

        public BtcCoreSerivce(NodeOptions nodeOptions)
        {
            _options = nodeOptions;
        }

        public Task<string> GetBestBlockHashAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Block> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeight(int height)
        {
            throw new NotImplementedException();
        }
    }
}
