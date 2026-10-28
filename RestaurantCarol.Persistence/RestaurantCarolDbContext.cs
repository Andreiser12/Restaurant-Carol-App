using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RestaurantCarol.Persistence;

public class RestaurantCarolDbContext
{
    private readonly string _connectionString;

    public RestaurantCarolDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("RestaurantDB")
            ?? throw new InvalidOperationException("Connection string 'RestaurantDB' not found.");
    }

    public SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    public string ConnectionString => _connectionString;
}
