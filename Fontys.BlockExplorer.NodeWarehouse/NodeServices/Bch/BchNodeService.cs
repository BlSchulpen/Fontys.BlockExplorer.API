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

        public Task<BchNodeBlock?> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        public Task<BchNodeRawTransaction?> GetRawTransactionAsync(string txId)
        {
            throw new NotImplementedException();
        }
    }
}
