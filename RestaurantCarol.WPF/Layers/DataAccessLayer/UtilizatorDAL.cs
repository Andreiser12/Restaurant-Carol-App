using System.Data;
using Microsoft.Data.SqlClient;

namespace RestaurantCarol.Layers
{
    public class UtilizatorDAL
    {
        public Utilizator? GetByEmail(string email)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetUtilizatorByEmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@email", email));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("CheckEmailExists", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@email", email));

                con.Open();
                object? result = cmd.ExecuteScalar();

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
                cmd.Parameters.Add(new SqlParameter("@parolaHash", utilizator.ParolaHash));
                cmd.Parameters.Add(new SqlParameter("@rol", utilizator.Rol.ToString()));

                SqlParameter paramId = new("@idUtilizator", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();

                utilizator.IdUtilizator = (int)paramId.Value;
            }
        }

        public int GetPuncteByUtilizator(int idUtilizator)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetPuncteByUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));

                con.Open();
                object? result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value) return 0;
                return (int)result;
            }
        }
    }
}