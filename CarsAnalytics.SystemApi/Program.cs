using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DataProvider
builder.Services.AddScoped<ITerritoryDataProvider, TerritoryDataProvider>();

// Service
builder.Services.AddScoped<ITerritoryService, TerritoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarsAnalytics API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
