﻿using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
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

        public BtcCoreService(IHttpClientFactory httpClientFactory, ILogger<BtcCoreService> logger)
        {
            _client = httpClientFactory.CreateClient("BtcCore");
            _logger = logger;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            const string content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getbestblockhash\"}";
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var formatted = json?[1..^2];
            return formatted;
        }

        public async Task<BtcBlockResponse> GetBlockFromHashAsync(string hash)
        {
            const int verbosity = 2;
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblock\",\"params\":[\"" + hash + "\"," + verbosity + "]}";
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"]?.ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BtcBlockResponse>(json);
            return responseObject;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblockhash\",\"params\":[" + height.ToString() + "]}";
            var response = await SendMessageAsync(content);
            var hash = JObject.Parse(response)["result"]?.ToString(Formatting.None);
            var formatted = Regex.Replace(hash, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return formatted;
        }

        public async Task<BtcTransactionResponse> GetRawTransactionAsync(string txId)
        {
            const bool returnObject = true;
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getrawtransaction\",\"params\":[\"" + txId + "\"," + returnObject.ToString().ToLower() + "]}";
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var responseObject = JsonConvert.DeserializeObject<BtcTransactionResponse>(json);
            return responseObject;
        }

        private async Task<string?> SendMessageAsync(string json)
        {
            try
            {
                var test2 = new StringContent(json);
                var test = _client.BaseAddress;
                var response = await _client.PostAsync(_client.BaseAddress, new StringContent(json));
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (NullReferenceException exception)
            {
                _logger.LogError(exception, "Connection to Btc Core could not be made");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "test");
                throw;
            }
        }
    }
}
