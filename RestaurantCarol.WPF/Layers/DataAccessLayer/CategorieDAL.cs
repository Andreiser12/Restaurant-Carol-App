using Microsoft.Data.SqlClient;
using RestaurantCarol.Layers;
using System.Collections.ObjectModel;
using System.Data;
namespace RestaurantCarol.Layers
{
    public class CategorieDAL
    {
        public ObservableCollection<Categorie> GetAllCategorii()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAllCategorii", connection);
                ObservableCollection<Categorie> result = new ObservableCollection<Categorie>();
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Categorie
                        {
                            IdCategorie = reader.GetInt32(0),
                            Denumire = reader.GetString(1),
                            Tip = GetTipCategorieFromDb(reader.GetString(2))
                        });
                    }
                }
                return result;
            }
        }
        public ObservableCollection<Categorie> GetCategoriiByTip(TipCategorie tip)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetCategoriiByTip", connection);
                ObservableCollection<Categorie> result = new ObservableCollection<Categorie>();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@tip", tip.ToString()));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Categorie
                        {
                            IdCategorie = reader.GetInt32(0),
                            Denumire = reader.GetString(1),
                            Tip = GetTipCategorieFromDb(reader.GetString(2))
                        });
                    }
                }
                return result;
            }
        }
        public void AddCategorie(Categorie categorie)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("AddCategorie", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@denumire", categorie.Denumire));
                command.Parameters.Add(new SqlParameter("@tip", categorie.Tip.ToString()));
                SqlParameter paramId = new("@idCategorie", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramId);
                connection.Open();
                command.ExecuteNonQuery();
                categorie.IdCategorie = (int)paramId.Value;
            }
        }
        public void ModifyCategorie(Categorie categorie)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("ModifyCategorie", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idCategorie", categorie.IdCategorie));
                command.Parameters.Add(new SqlParameter("@denumire", categorie.Denumire));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeleteCategorie(Categorie categorie)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("DeleteCategorie", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idCategorie", categorie.IdCategorie));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        private TipCategorie GetTipCategorieFromDb(string tipDb)
        {
            if (string.Equals(tipDb, "bautura", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(tipDb, "bauturi", StringComparison.OrdinalIgnoreCase))
            {
                return TipCategorie.Bauturi;
            }
            return TipCategorie.Mancare;
        }
    }
}
