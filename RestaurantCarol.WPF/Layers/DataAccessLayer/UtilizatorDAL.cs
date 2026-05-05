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
                            AdresaLivrare = reader.IsDBNull(reader.GetOrdinal("AdresaLivrare"))
                                ? null : reader.GetString(reader.GetOrdinal("AdresaLivrare")),
                            ParolaHash = reader.GetString(reader.GetOrdinal("ParolaHash")),
                            RolUtilizator = Enum.Parse<RolUtilizator>(reader.GetString(reader.GetOrdinal("Rol")))
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
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("AddUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@nume", utilizator.Nume));
                cmd.Parameters.Add(new SqlParameter("@prenume", utilizator.Prenume));
                cmd.Parameters.Add(new SqlParameter("@email", utilizator.Email));
                cmd.Parameters.Add(new SqlParameter("@telefon",
                    (object?)utilizator.Telefon ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@adresaLivrare",
                    (object?)utilizator.AdresaLivrare ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@parolaHash", utilizator.ParolaHash));
                cmd.Parameters.Add(new SqlParameter("@rol", utilizator.RolUtilizator.ToString()));

                SqlParameter paramId = new("@idUtilizator", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();

                utilizator.IdUtilizator = (int)paramId.Value;
            }
        }


    }
}