using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Bch
{
    public class BchNodeService : IBchNodeService
    {
        public Task<string?> GetBestBlockHashAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BchNodeBlock?> GetBlockFromHash(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        public Task<BchNodeRawTransaction?> GetRawTransaction(string txId)
        {
            throw new NotImplementedException();
        }
    }
}
