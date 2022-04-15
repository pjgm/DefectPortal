using Application;
using Carter;
using Infrastructure;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCarter(configurator: c => c.WithEmptyValidators())
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app
        .UseSeedData()
        .UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapCarter();

await app.RunAsync();
