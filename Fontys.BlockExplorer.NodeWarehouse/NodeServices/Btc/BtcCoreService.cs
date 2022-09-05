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

        public Task<Block> GetBlockFromHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashFromHeightAsync(int height)
        {
            throw new NotImplementedException();
        }

        private async Task<string> SendMessageAsync(string json)
        {
            var username = _options.Username; //TODO: azure key vault
            var password = _options.Password;//TODO: azure key vault
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            var response = await client.PostAsync(_options.BaseUrl ,new StringContent(json));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
