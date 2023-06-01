using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.RawTransaction;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    public interface IBtcNodeService
    {
        Task<string?> GetBestBlockHashAsync();
        Task<string?> GetHashFromHeightAsync(int height);
        Task<BtcBlockResponse?> GetBlockFromHashAsync(string hash);
        Task<BtcTransactionResponse?> GetRawTransactionAsync(string txId);
    }
}
