using Autofac;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;

namespace Fontys.BlockExplorer.API.Modules
{
    public class MonitoringModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExplorerNodeMonitoringService>().As<INodeMonitoringService>();
            builder.RegisterType<ExplorerAddressRestoreService>().As<IAddressRestoreService>();
            builder.RegisterType<BtcBlockProviderService>().As<IBlockDataProviderService>();
        }
    }
}
