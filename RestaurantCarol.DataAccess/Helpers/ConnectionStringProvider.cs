using System.Configuration;

namespace RestaurantCarol.DataAccess.Helpers;

public static class ConnectionStringProvider
{
    public static string GetConnectionString()
    {
        var cs = ConfigurationManager.ConnectionStrings["RestaurantDB"]?.ConnectionString;

        if (string.IsNullOrEmpty(cs))
        {
            throw new InvalidOperationException(
                "Connection string 'RestaurantDB' nu a fost gasit in App.config. " +
                "Verifica daca App.config exista si este corect configurat.");
        }

        return cs;
    }
}