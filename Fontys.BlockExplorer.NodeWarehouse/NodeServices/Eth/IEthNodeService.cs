using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fontys.BlockExplorer.Domain.CoinResponseModels.EthGeth;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth
{
    public interface IEthNodeService
    {
        Task<EthBlockResponse> GetBlockByHashAsync(string hash);
        Task<EthBlockResponse> GetBlockByNumberAsync(int number);
        Task<int> GetLatestNumber();

    }
}
