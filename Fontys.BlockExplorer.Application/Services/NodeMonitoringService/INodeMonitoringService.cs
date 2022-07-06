namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    using Fontys.BlockExplorer.Domain.Models;
    using System.Threading.Tasks;
    
    public interface INodeMonitoringService
    {
        Task<ICollection<Block>> RemoveBadBlocksAsync();
        Task<ICollection<Block>> GetNewBlocksAsync();
    }
}
