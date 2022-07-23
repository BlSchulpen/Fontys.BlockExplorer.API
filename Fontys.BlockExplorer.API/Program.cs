using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.API.Modules;
using Fontys.BlockExplorer.API.Profiles;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.PostgresDb;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.Configure<PostgresDbOptions>(builder.Configuration.GetRequiredSection(nameof(PostgresDbOptions)));
builder.Services.AddDbContext<BlockExplorerContext,PostgresDatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"), b => b.MigrationsAssembly("Fontys.BlockExplorer.API")));

builder.Services.AddAutoMapper(typeof(ExplorerProfile));

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ExplorerModule()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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