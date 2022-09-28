using AutoMapper;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.PostgresDb;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeDataManager.Workers;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Cryptography;
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

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.Configure<PostgresDbOptions>(builder.Configuration.GetRequiredSection(nameof(PostgresDbOptions)));
builder.Services.AddScoped<INodeService, BtcCoreService>();
builder.Services.AddScoped<INodeMonitoringService, ExplorerNodeMonitoringService>();
builder.Services.AddScoped<IAddressRestoreService, ExplorerAddressRestoreService>();
builder.Services.AddDbContext<BlockExplorerContext, PostgresDatabaseContext>(options => options.UseNpgsql(builder.Configuration["PostgresDbOptions:ConnectionsString"], b => b.MigrationsAssembly("Fontys.BlockExplorer.API")));
builder.Services.AddHostedService<NodeDataWorker>();



// Automappers
builder.Services.AddAutoMapper(typeof(BtcProfile));

// Http Clients
builder.Services.AddHttpClient("BtcCore", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["BtcCoreSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["BtcCoreSettings:Username"]}:{builder.Configuration["BtcCoreSettings:Password"]}")));
});


var app = builder.Build();
app.Run();
