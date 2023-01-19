var builder = WebApplication.CreateBuilder(args);

#region Adding services to container

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region WebApp scoped services

// Scoped services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // EFCore

    // DB seed and update
}

#endregion

#region WebApp MiddleWare

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
