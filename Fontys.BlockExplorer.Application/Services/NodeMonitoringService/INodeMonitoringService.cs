using Fontys.BlockExplorer.Domain.Models;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{   
    public interface INodeMonitoringService
    {
        Task<ICollection<Block>> RemoveBadBlocksAsync();
        Task<ICollection<Block>> GetNewBlocksAsync();
    }
}
