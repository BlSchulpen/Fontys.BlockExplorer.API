using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.EthGeth;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth
{
    public interface IEthNodeService
    {
        Task<EthBlockResponse> GetBlockByHashAsync(string hash);
        Task<EthBlockResponse> GetBlockByNumberAsync(int number);
        Task<int> GetLatestNumber();

    }
}
