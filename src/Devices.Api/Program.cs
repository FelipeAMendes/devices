using Devices.Api.DependencyInjections;
using Devices.Application.DependencyInjections;
using Devices.Infrastructure.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

app.Run();
