using HotChocolate.AspNetCore;
using InventoryAPI.API.GraphQL;
using InventoryAPI.Application;
using InventoryAPI.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);



builder.Services.AddControllers();
builder.Services.AddGraphQLServer().AddQueryType<Query>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Inventory API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL().WithOptions(options =>
{
    options.Tool.Enable = app.Environment.IsDevelopment();
});

app.Run();
