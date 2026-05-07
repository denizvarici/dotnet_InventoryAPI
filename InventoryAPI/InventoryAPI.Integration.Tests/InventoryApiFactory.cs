using InventoryAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;

namespace InventoryAPI.Integration.Tests;

public class InventoryApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("InventoryTestDb")
        .WithUsername("postgres")
        .WithPassword("testpassword123")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<InventoryDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<InventoryDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            db.Database.Migrate();
        });
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();
    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}