using Microsoft.Data.SqlClient;
using RestaurantCarol.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace RestaurantCarol.Layers
{
    public class CategorieDAL
    {
        public ObservableCollection<Categorie> GetAllCategorii()
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAllCategorii", con);
                ObservableCollection<Categorie> result = new ObservableCollection<Categorie>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Categorie
                        {
                            IdCategorie = reader.GetInt32(0),
                            Denumire = reader.GetString(1)
                        });
                    }
                }
                return result;
            }
        }
        // Add, Modify, Delete vor veni
    }
}