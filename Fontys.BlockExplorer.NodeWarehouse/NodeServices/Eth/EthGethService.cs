using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.Domain.CoinResponseModels.EthGeth;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth
{
    //TODO
    public class EthGethService : IEthNodeService
    {
        public Task<EthBlockResponse> GetBlockByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<EthBlockResponse> GetBlockByNumberAsync(int number)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLatestNumber()
        {
            throw new NotImplementedException();
        }
    }
    }
}
