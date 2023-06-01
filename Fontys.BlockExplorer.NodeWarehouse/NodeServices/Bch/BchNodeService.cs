using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;
using Fontys.BlockExplorer.NodeWarehouse.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
            var arguments = new Dictionary<string, string>
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "curltest",
                ["method"] = "getbestblockhash"
            };
            var contentParams = new List<string>();
            var content = RpcContentBuilderExtension.RpcContent(arguments, contentParams);
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var formatted = json?[1..^2];
            return formatted;
        }

        public async Task<BchBlockResponse> GetBlockFromHashAsync(string hash)
        {
            var arguments = new Dictionary<string, string>
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "curltest",
                ["method"] = "getblock"
            };
            var contentParams = new List<string> {
                hash
            };
            var content = RpcContentBuilderExtension.RpcContent(arguments, contentParams);
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BchBlockResponse>(json);
            return responseObject;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var arguments = new Dictionary<string, string>
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "curltest",
                ["method"] = "getblockhash"
            };
            var contentParams = new List<string> { height.ToString() };
            var content = RpcContentBuilderExtension.RpcContent(arguments, contentParams);
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var hash = JObject.Parse(response)["result"]?.ToString(Formatting.None);
            var formatted = Regex.Replace(hash, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return formatted;
        }

        public async Task<BchRawTransactionResponse> GetRawTransactionAsync(string txId)
        {
            const bool returnObject = true;
            var arguments = new Dictionary<string, string>
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "curltest",
                ["method"] = "getrawtransaction"
            };
            var contentParams = new List<string> { txId, returnObject.ToString().ToLower(), true.ToString() }; //TODO it is possible not all params are stored as string
            var content = RpcContentBuilderExtension.RpcContent(arguments, contentParams);
            var response = await SendMessageAsync(content);
            if (response == null)
                return null;
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BchRawTransactionResponse>(json);
            return responseObject;
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
