namespace Fontys.BlockExplorer.API.Modules
{
    using Autofac;
    using Fontys.BlockExplorer.Application.Services.AddressService;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Fontys.BlockExplorer.Application.Services.TxService;
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Data.PostgresDb;

    public class ExplorerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostgresDatabaseContext>().As<BlockExplorerContext>();
            builder.RegisterType<ExplorerBlockService>().As<IBlockService>();
            builder.RegisterType<ExplorerTxService>().As<ITxService>();
            builder.RegisterType<ExplorerAddressService>().As<IAddressService>();
        }
    }
}
