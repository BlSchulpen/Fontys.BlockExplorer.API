using Fontys.BlockExplorer.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Fontys.BlockExplorer.API.Hubs
{
    public class BtcBlockHub : Hub
    {
        public async Task SendMessage(Block block)
        { 
            await Clients.All.SendAsync(JsonConvert.SerializeObject(block));
        }

    }
}
