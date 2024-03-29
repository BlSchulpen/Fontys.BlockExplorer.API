using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.PostgresDb;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeDataManager.Workers;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Inject provider types
builder.Services.AddScoped<BtcBlockProviderService>();

builder.Services.AddTransient<Func<CoinType, IBlockDataProviderService?>>(blockProviderType => key =>
{
    return key switch
    {
        CoinType.BTC => blockProviderType.GetService<BtcBlockProviderService>(),
        _ => null
    };
});

// keep local app-settings file
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.Configure<PostgresDbOptions>(builder.Configuration.GetRequiredSection(nameof(PostgresDbOptions)));
builder.Services.AddScoped<IBtcNodeService, BtcCoreService>();
builder.Services.AddScoped<IEthNodeService, EthGethService>();
builder.Services.AddScoped<INodeMonitoringService, ExplorerNodeMonitoringService>();
builder.Services.AddScoped<IAddressRestoreService, ExplorerAddressRestoreService>();
builder.Services.AddDbContext<BlockExplorerContext, PostgresDatabaseContext>(options => options.UseNpgsql(builder.Configuration["PostgresDbOptions:ConnectionsString"], b => b.MigrationsAssembly("Fontys.BlockExplorer.API")));
builder.Services.AddHostedService<NodeDataWorker>();



// Automappers
builder.Services.AddAutoMapper(typeof(BtcProfile));
builder.Services.AddAutoMapper(typeof(EthProfile));

builder.Services.AddHttpClient("BtcCore", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["BtcCoreSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["BtcCoreSettings:Username"]}:{builder.Configuration["BtcCoreSettings:Password"]}")));
});

builder.Services.AddHttpClient("EthGeth", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["EthGethSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["EthGethSettings:Username"]}:{builder.Configuration["EthGethSettings:Password"]}")));
});

var app = builder.Build();
app.Run();
