using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.EthGeth;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth
{
    //TODO
    public class EthGethService : IEthNodeService
    {
        private static HttpClient _client;
        private readonly ILogger<EthGethService> _logger;

        public EthGethService(IHttpClientFactory httpClientFactory, ILogger<EthGethService> logger)
        {
            _client = httpClientFactory.CreateClient("BtcCore");
            _logger = logger;
        }
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