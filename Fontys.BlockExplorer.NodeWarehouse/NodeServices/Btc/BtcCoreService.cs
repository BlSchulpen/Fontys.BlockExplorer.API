namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class BtcCoreService : INodeService
    {
        private readonly BtcOptions _options;
        private static HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;

        public BtcCoreService(IOptions<BtcOptions> nodeOptions, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("BtcCore");
            _options = nodeOptions.Value;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getbestblockhash\"}"; //todo improve custom content class
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var formated = json.Substring(1, json.Length - 2);
            return formated;
        }

        public async Task<Block> GetBlockFromHashAsync(string hash)
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblock\",\"params\":[\"" + hash + "\",2]}";
            var response = await SendMessageAsync(content);
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            try
            {
                var test = JsonConvert.DeserializeObject<BtcBlockResponse>(json);
                var test2 = 0;
            }
            catch (Exception e)
            {
                throw;
            }
                var responseBlock = JsonConvert.DeserializeObject<Block>(json); 
            return responseBlock;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var content = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblockhash\",\"params\":[" + height.ToString() + "]}";
            var response = await SendMessageAsync(content);
            var hash = JObject.Parse(response)["result"].ToString(Formatting.None);
            hash = hash.Substring(1, hash.Length - 2);
            return hash;
        }

        private async Task<string> SendMessageAsync(string json)
        {
            var response = await _client.PostAsync(_client.BaseAddress ,new StringContent(json));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
