using System.Data;
using InventoryAPI.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InventoryAPI.Infrastructure.Persistence;
using Npgsql;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection string bulunamadı!");
    }


    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}