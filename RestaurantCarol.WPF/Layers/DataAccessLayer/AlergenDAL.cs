using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class AlergenDAL
    {
        public ObservableCollection<Alergen> GetAllAlergeni()
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAllAlergeni", con);
                ObservableCollection<Alergen> result = [];
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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