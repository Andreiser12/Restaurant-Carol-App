using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class AdresaDAL
    {
        public ObservableCollection<Adresa> GetByUtilizator(int idUtilizator)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAdreseByUtilizator", con);
                ObservableCollection<Adresa> result = [];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAdresaImplicita", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("AddAdresa", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idUtilizator", adresa.IdUtilizator));
                cmd.Parameters.Add(new SqlParameter("@adresa", adresa.AdresaText));
                cmd.Parameters.Add(new SqlParameter("@esteImplicita", adresa.EsteImplicita));
                SqlParameter paramId = new("@idAdresa", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramId);
                con.Open();
                cmd.ExecuteNonQuery();
                adresa.IdAdresa = (int)paramId.Value;
            }
        }
        public void ModifyAdresa(Adresa adresa)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("ModifyAdresa", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idAdresa", adresa.IdAdresa));
                cmd.Parameters.Add(new SqlParameter("@adresa", adresa.AdresaText));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void SetImplicita(int idAdresa)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("SetAdresaImplicita", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idAdresa", idAdresa));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteAdresa(int idAdresa)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("DeleteAdresa", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idAdresa", idAdresa));
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}