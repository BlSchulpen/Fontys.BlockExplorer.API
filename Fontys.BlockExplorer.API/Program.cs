using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fontys.BlockExplorer.API.Modules;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ExplorerModule()));
builder.Services.AddDbContext<BlockExplorerContext>(options => options.UseNpgsql("User ID=postgres;Password=Explorer;Host=localhost;Port=5432;Database=ExplorerDb;")); // todo fix connection string from appsettings

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


/*
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BlockExplorerContext>();
    dbContext.Database.Migrate();
}
*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
