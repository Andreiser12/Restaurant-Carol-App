using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class AlergenDAL
    {
        public ObservableCollection<Alergen> GetAllAlergeni()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAllAlergeni", connection);
                ObservableCollection<Alergen> result = new ObservableCollection<Alergen>();
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Alergen
                        {
                            IdAlergen = reader.GetInt32(reader.GetOrdinal("IdAlergen")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire"))
                        });
                    }
                }
                return result;
            }
        }
    }
}