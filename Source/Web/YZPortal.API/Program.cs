using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Infrastructure.Extensions;
using Application.Extensions;
using YZPortal.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Logging

builder.Logging.ConfigureLogging();

#endregion

#region Add services

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);
builder.Services.AddWebLayer(builder.Configuration);

#endregion

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

app.UseInfrastructure(builder.Services);

app.UseSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), builder.Configuration);

app.MapEndpoints();

app.Run();

