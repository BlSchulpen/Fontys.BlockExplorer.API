using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{   
    public interface INodeMonitoringService
    {
        Task<ICollection<Block>> RemoveBadBlocksAsync(CoinType coinType);
        Task<ICollection<Block>> GetNewBlocksAsync(CoinType coinType);
    }
}
