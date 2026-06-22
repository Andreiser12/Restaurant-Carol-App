using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class AdresaDAL
    {
        public ObservableCollection<Adresa> GetByUtilizator(int idUtilizator)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAdreseByUtilizator", connection);
                ObservableCollection<Adresa> result = [];
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Adresa
                        {
                            IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                            IdUtilizator = reader.GetInt32(reader.GetOrdinal("IdUtilizator")),
                            AdresaText = reader.GetString(reader.GetOrdinal("Adresa")),
                            EsteImplicita = reader.GetBoolean(reader.GetOrdinal("EsteImplicita"))
                        });
                    }
                }
                return result;
            }
        }

        public Adresa? GetAdresaImplicita(int idUtilizator)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAdresaImplicita", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Adresa
                        {
                            IdAdresa = reader.GetInt32(reader.GetOrdinal("IdAdresa")),
                            IdUtilizator = reader.GetInt32(reader.GetOrdinal("IdUtilizator")),
                            AdresaText = reader.GetString(reader.GetOrdinal("Adresa")),
                            EsteImplicita = reader.GetBoolean(reader.GetOrdinal("EsteImplicita"))
                        };
                    }
                    return null;
                }
            }
        }

        public void AddAdresa(Adresa adresa)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("AddAdresa", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", adresa.IdUtilizator));
                command.Parameters.Add(new SqlParameter("@adresa", adresa.AdresaText));
                command.Parameters.Add(new SqlParameter("@esteImplicita", adresa.EsteImplicita));
                SqlParameter paramId = new("@idAdresa", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramId);
                connection.Open();
                command.ExecuteNonQuery();
                adresa.IdAdresa = (int)paramId.Value;
            }
        }

        public void ModifyAdresa(Adresa adresa)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("ModifyAdresa", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idAdresa", adresa.IdAdresa));
                command.Parameters.Add(new SqlParameter("@adresa", adresa.AdresaText));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SetImplicita(int idAdresa)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("SetAdresaImplicita", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idAdresa", idAdresa));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAdresa(int idAdresa)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("DeleteAdresa", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idAdresa", idAdresa));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}