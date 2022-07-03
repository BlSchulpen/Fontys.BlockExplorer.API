namespace Fontys.BlockExplorer.NodeDataManager.Workers
{
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Microsoft.Extensions.Hosting;

    public class NodeDataWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INodeMonitoringService _nodeService;

        public NodeDataWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            using var scope = _scopeFactory.CreateScope();
            _nodeService = scope.ServiceProvider.GetRequiredService<INodeMonitoringService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
                _nodeService.UpdateStoredAsync();
            }
        }
    }
}
