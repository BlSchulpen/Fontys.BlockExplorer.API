using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Bch
{
    public interface IBchNodeService
    {
        Task<string?> GetBestBlockHashAsync();

        Task<string?> GetHashFromHeightAsync(int height);
        Task<BchNodeBlock?> GetBlockFromHash(string hash);

        Task<BchNodeRawTransaction?> GetRawTransaction(string txId);

    }
}
