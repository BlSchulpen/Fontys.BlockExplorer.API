using Autofac.Extensions.DependencyInjection;
using Autofac;
using Fontys.BlockExplorer.API.Modules;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.PostgresDb;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeDataManager.Workers;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;

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

// Add services to the container.R
builder.Services.AddHttpClient();
builder.Services.Configure<PostgresDbOptions>(builder.Configuration.GetRequiredSection(nameof(PostgresDbOptions)));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MonitoringModule()));
builder.Services.AddDbContext<BlockExplorerContext, PostgresDatabaseContext>(options => options.UseNpgsql(builder.Configuration["PostgresDbOptions:ConnectionsString"], b => b.MigrationsAssembly("Fontys.BlockExplorer.API")));
builder.Services.AddHostedService<NodeDataWorker>();
builder.Services.AddAutoMapper(typeof(BtcProfile));
builder.Services.AddScoped<IBtcNodeService, BtcCoreService>();
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ExplorerModule()));

//HttpClients
builder.Services.AddHttpClient("BtcCore", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["BtcCoreSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["BtcCoreSettings:Username"]}:{builder.Configuration["BtcCoreSettings:Password"]}")));
});

builder.Services.AddHttpClient("BchNode", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["BchNodeSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["BchNodeSettings:Username"]}:{builder.Configuration["BchNodeSettings:Password"]}")));
});


var app = builder.Build();
app.Run();
