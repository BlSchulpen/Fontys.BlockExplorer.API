namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
    using Fontys.BlockExplorer.Domain.Models;

    public interface IBtcNodeService
    {
        Task<string> GetBestBlockHashAsync();
        Task<string> GetHashFromHeightAsync(int height);
        Task<BtcBlockResponse> GetBlockFromHashAsync(string hash);
        Task<BtcTransactionResponse> GetRawTransactionAsync(string txId);
    }
}
