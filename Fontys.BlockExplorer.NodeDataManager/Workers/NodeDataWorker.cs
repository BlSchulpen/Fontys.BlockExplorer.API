namespace Fontys.BlockExplorer.NodeDataManager.Workers
{
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.Domain.Enums;
    using Microsoft.Extensions.Hosting;

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
                        foreach (var coinType in Enum.GetValues(typeof(CoinType)).Cast<CoinType>())
                        {
             //               await nodeService.RemoveBadBlocksAsync(coinType);
               //             await nodeService.GetNewBlocksAsync(coinType);
                        }
                    }
                }
            }
        }
    }
}
