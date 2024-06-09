using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Infrastructure.Extensions;
using Application.Extensions;
using YZPortal.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region Logging

builder.Logging.ConfigureLogging();
builder.Host.UseSerilog(configuration);

#endregion

#region Add services

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(configuration, builder.Environment);
builder.Services.AddWebLayer(configuration);

#endregion

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

app.UseInfrastructure(builder.Services);

app.UseSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), configuration);

app.MapEndpoints();

app.Run();

