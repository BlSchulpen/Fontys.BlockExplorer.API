namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class BtcCoreService : IExplorerBtcService
    {
        private readonly btcOptions _options;

        public BtcCoreService(IOptions<btcOptions> nodeOptions)
        {
            _options = nodeOptions.Value;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var addition = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getbestblockhash\"}";
            var response = await SendMessageAsync(addition);
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var formated = json.Substring(1, json.Length - 2);
            return formated;
        }

        public async Task<Block> GetBlockFromHashAsync(string hash)
        {
            var addition = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblock\",\"params\":[\"" + hash + "\",2]}";
            var response = await SendMessageAsync(addition);
            var json = JObject.Parse(response)["result"].ToString(Formatting.Indented);
            var responseBlock = JsonConvert.DeserializeObject<Block>(json); //todo test if this works
            return responseBlock;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var addition = "{\"jsonrpc\":\"1.0\",\"id\":\"1\",\"method\":\"getblockhash\",\"params\":[" + height.ToString() + "]}";
            var response = await SendMessageAsync(addition);
            var hash = JObject.Parse(response)["result"].ToString(Formatting.None);
            hash = hash.Substring(1, hash.Length - 2);
            return hash;
        }

        private async Task<string> SendMessageAsync(string json)
        {
            var username = _options.Username; 
            var password = _options.Password;
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            var response = await client.PostAsync(_options.BaseUrl ,new StringContent(json));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
