using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class UtilizatorDAL
    {
        public Utilizator? GetByEmail(string email)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetUtilizatorByEmail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@email", email));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilizator
                        {
                            IdUtilizator = reader.GetInt32(reader.GetOrdinal("IdUtilizator")),
                            Nume = reader.GetString(reader.GetOrdinal("Nume")),
                            Prenume = reader.GetString(reader.GetOrdinal("Prenume")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefon = reader.IsDBNull(reader.GetOrdinal("Telefon"))
                                ? null : reader.GetString(reader.GetOrdinal("Telefon")),
                            ParolaHash = reader.GetString(reader.GetOrdinal("ParolaHash")),
                            Rol = Enum.Parse<RolUtilizator>(reader.GetString(reader.GetOrdinal("Rol"))),
                            Puncte = reader.GetInt32(reader.GetOrdinal("Puncte"))
                        };
                    }
                    return null;
                }
            }
        }
        public bool CheckEmailExists(string email)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("CheckEmailExists", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@email", email));
                connection.Open();
                object? result = command.ExecuteScalar();
                if (result == null) return false;
                return (int)result == 1;
            }
        }
        public void AddUtilizator(Utilizator utilizator)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("AddUtilizator", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nume", utilizator.Nume));
                command.Parameters.Add(new SqlParameter("@prenume", utilizator.Prenume));
                command.Parameters.Add(new SqlParameter("@email", utilizator.Email));
                command.Parameters.Add(new SqlParameter("@telefon",
                    (object?)utilizator.Telefon ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@parolaHash", utilizator.ParolaHash));
                command.Parameters.Add(new SqlParameter("@rol", utilizator.Rol.ToString()));
                SqlParameter paramId = new("@idUtilizator", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramId);
                connection.Open();
                command.ExecuteNonQuery();
                utilizator.IdUtilizator = (int)paramId.Value;
            }
        }
        public int GetPuncteByUtilizator(int idUtilizator)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetPuncteByUtilizator", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                connection.Open();
                object? result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value) return 0;
                return (int)result;
            }
        }
    }
}