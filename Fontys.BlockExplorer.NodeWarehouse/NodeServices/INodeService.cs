namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices
{
    using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;

    public interface INodeService
    {
        Task<string> GetBestBlockHashAsync();
        Task<string> GetHashFromHeightAsync(int height);
        Task<BtcBlockResponse> GetBlockFromHashAsync(string hash);
        Task<BtcTransactionResponse> GetRawTransactionAsync(string txId);
    }
}
