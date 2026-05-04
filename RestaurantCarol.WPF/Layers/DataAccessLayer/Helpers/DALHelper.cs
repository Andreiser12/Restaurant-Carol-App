using System.Configuration;
using Microsoft.Data.SqlClient;

namespace RestaurantCarol.Layers
{
    public class DALHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        public static SqlConnection Connection
        {
            get => new SqlConnection(connectionString);
        }
    }
}
