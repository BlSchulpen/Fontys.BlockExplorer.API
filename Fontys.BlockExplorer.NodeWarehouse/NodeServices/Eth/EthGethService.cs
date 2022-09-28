using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth
{
    public class EthGethService : INodeService
    {
        public Task<string> GetBestBlockHashAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        public Task<BtcBlockResponse> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<BtcTransactionResponse> GetRawTransactionAsync(string txId)
        {
            throw new NotImplementedException();
        }
    }
}
