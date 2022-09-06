using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.PostgresDb;
using Fontys.BlockExplorer.NodeDataManager.Workers;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.Configure<BtcOptions>(builder.Configuration.GetSection("NodeCommands"));
builder.Services.Configure<PostgresDbOptions>(builder.Configuration.GetRequiredSection(nameof(PostgresDbOptions)));
builder.Services.AddScoped<INodeService, BtcCoreService>();
builder.Services.AddScoped<INodeMonitoringService, ExplorerMonitoringService>();
builder.Services.AddDbContext<BlockExplorerContext, PostgresDatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"), b => b.MigrationsAssembly("Fontys.BlockExplorer.API")));
builder.Services.AddHostedService<NodeDataWorker>();

// Http Clients
builder.Services.AddHttpClient("BtcCore", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["BtcCoreSettings:BaseUrl"]);
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{builder.Configuration["BtcCoreSettings:Username"]}:{builder.Configuration["BtcCoreSettings:Password"]}")));
});


var app = builder.Build();
app.Run();
