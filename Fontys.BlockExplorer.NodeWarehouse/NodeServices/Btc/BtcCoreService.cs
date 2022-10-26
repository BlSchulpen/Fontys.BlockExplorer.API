using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.RawTransaction;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    public class BtcCoreService : IBtcNodeService
    {
        private static HttpClient _client;
        private readonly ILogger<BtcCoreService> _logger;

        //TODO fix code quality, factory for request content
        public BtcCoreService(IHttpClientFactory httpClientFactory, ILogger<BtcCoreService> logger)
        {
            _client = httpClientFactory.CreateClient("BtcCore");
            _logger = logger;
        }

        //TODO how to handle not found exceptions after sending message
        public async Task<string?> GetBestBlockHashAsync()
        {
            const string content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getbestblockhash\"}";
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var formatted = json?[1..^2];
            return formatted;
        }

        public async Task<BtcBlockResponse> GetBlockFromHashAsync(string hash)
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblock\",\"params\":[\"" + hash + "\"," + 2 + "]}";
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BtcBlockResponse>(json);
            return responseObject;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblockhash\",\"params\":[" + height.ToString() + "]}";
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var hash = JObject.Parse(response)["result"]?.ToString(Formatting.None);
            var formatted = Regex.Replace(hash, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return formatted;
        }

        public async Task<BtcTransactionResponse> GetRawTransactionAsync(string txId)
        {
            const bool returnObject = true;
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getrawtransaction\",\"params\":[\"" + txId + "\"," + returnObject.ToString().ToLower() + "]}";
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BtcTransactionResponse>(json);
            return responseObject;
        }

        //!TODO clean way of doing this
        private string RpcContent(Dictionary<string, string> dictArgumentsValue,
            Dictionary<string, string> dictParamsValue)
        {
            return "test";
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
