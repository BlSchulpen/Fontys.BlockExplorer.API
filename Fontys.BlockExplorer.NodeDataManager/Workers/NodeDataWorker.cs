namespace Fontys.BlockExplorer.NodeDataManager.Workers
{
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
    using Microsoft.Extensions.Hosting;
    using System.Collections.Specialized;

    public class NodeDataWorker : BackgroundService
    {
        private readonly IServiceProvider _service;

        public NodeDataWorker(IServiceProvider service)
        {
            _service = service;
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
                        try
                        {
                            await nodeService.RemoveBadBlocksAsync();
                        }

                        catch (Exception e)
                        {
                            return;
                        }
                            await nodeService.GetNewBlocksAsync();
                    }
                }
            }
        }
    }
}
