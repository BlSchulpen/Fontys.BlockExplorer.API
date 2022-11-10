using Autofac;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;

namespace Fontys.BlockExplorer.API.Modules
{
    public class MonitoringModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
          //  builder.RegisterType<IBtcNodeService>().As<BtcCoreService>();
     //       builder.RegisterType<IEthNodeService>().As<EthGethService>();
            builder.RegisterType<INodeMonitoringService>().As<ExplorerNodeMonitoringService>();
            builder.RegisterType<IAddressRestoreService>().As<ExplorerAddressRestoreService>();
        }
    }
}
