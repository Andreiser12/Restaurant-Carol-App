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
    }
}