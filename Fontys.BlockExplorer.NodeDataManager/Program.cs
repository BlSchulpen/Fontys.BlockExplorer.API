using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.NodeDataManager.Workers;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<btcOptions>(builder.Configuration.GetSection("NodeCommands"));

//builder.Services.AddScoped<INodeService, BtcCoreService>();

builder.Services.AddScoped<INodeService, BtcCoreService>();


builder.Services.AddScoped<INodeMonitoringService, ExplorerMonitoringService>();

builder.Services.AddDbContext<BlockExplorerContext>(options => options.UseNpgsql("User ID=postgres;Password=Explorer;Host=localhost;Port=5432;Database=ExplorerDb;")); // todo fix connection string from appsettings

builder.Services.AddHostedService<NodeDataWorker>();


var app = builder.Build();


app.Run();
