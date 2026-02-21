using CarsAnalytics.SystemApi.Data;
using CarsAnalytics.SystemApi.DataProviders;
using CarsAnalytics.SystemApi.DataProviders.Interfaces;
using CarsAnalytics.SystemApi.Services;
using CarsAnalytics.SystemApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DataProvider
builder.Services.AddScoped<ITerritoryDataProvider, TerritoryDataProvider>();
builder.Services.AddScoped<ICarModelDataProvider, CarModelDataProvider>();

// Service
builder.Services.AddScoped<ITerritoryService, TerritoryService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();

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
