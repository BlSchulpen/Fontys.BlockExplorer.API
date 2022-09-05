namespace Fontys.BlockExplorer.NodeDataManager.Workers
{
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
    using Microsoft.Extensions.Hosting;

    public class NodeDataWorker : BackgroundService
    {
        //  private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _service;

        private readonly INodeMonitoringService _nodeService;

        public NodeDataWorker(IServiceProvider service)
        {
            _service = service;
            // _nodeService.RemoveBadBlocksAsync();

        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
                {
                    using (var scope = _service.CreateScope())
                    {
                        var nodeService = scope.ServiceProvider.GetRequiredService<INodeMonitoringService>();
                        await nodeService.RemoveBadBlocksAsync();
                        await nodeService.GetNewBlocksAsync();
                    }
                }
            }
        }
    }
}
