namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    using System.Threading.Tasks;
    
    public interface INodeMonitoringService
    {
        Task RemoveBadBlocksAsync();
        Task GetNewBlocksAsync();
    }
}
