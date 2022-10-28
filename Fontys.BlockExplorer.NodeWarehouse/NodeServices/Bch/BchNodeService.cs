using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Bch
{
    public class BchNodeService : IBchNodeService
    {
        private readonly ILogger<IBchNodeService> _logger;
        private readonly HttpClient _client;

        public BchNodeService(ILogger<IBchNodeService> logger, HttpClient client)
        { 
            _logger = logger;
            _client = client;
        }

        public async Task<string> GetBestBlockHashAsync()
        {                

            throw new NotImplementedException();
        }

        public Task<BchBlockResponse> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        public Task<BchRawTransactionResponse> GetRawTransactionAsync(string txId)
        {
            throw new NotImplementedException();
        }

        private async Task<string?> SendMessageAsync(string json)
        {
            _logger.LogInformation("sending the following json message to client: {Json}", json);
            try
            {
                var response = await _client.PostAsync(_client.BaseAddress, new StringContent(json));
                _logger.LogInformation("Response message: {Response}", response);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception exception)
            {
                _logger.LogError("Message could not be send the following error was thrown {Exception}", exception); // change "Thrown"
                return null;
            }
        }
    }
}
