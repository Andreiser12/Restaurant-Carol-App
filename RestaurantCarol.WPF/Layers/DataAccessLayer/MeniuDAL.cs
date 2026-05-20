using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class MeniuDAL
    {
        public ObservableCollection<Meniu> GetByCategorie(int idCategorie)
        {
            using SqlConnection connection = DALHelper.Connection;
            SqlCommand command = new("GetMeniuriByCategorie", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@idCategorie", idCategorie));
            ObservableCollection<Meniu> meniuri = new();
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                meniuri.Add(new Meniu
                {
                    IdMeniu = Convert.ToInt32(reader["IdMeniu"]),
                    Denumire = reader["Denumire"].ToString() ?? string.Empty,
                    IdCategorie = Convert.ToInt32(reader["IdCategorie"])
                });
            }
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    int idMeniu = Convert.ToInt32(reader["IdMeniu"]);
                    Meniu? meniu = meniuri.FirstOrDefault(m => m.IdMeniu == idMeniu);
                    if (meniu == null) continue;
                    var componenta = new MeniuPreparatItem
                    {
                        IdMeniu = idMeniu,
                        IdPreparat = Convert.ToInt32(reader["IdPreparat"]),
                        CantitatePortie = Convert.ToDecimal(reader["CantitatePortie"]),
                        Preparat = new Preparat
                        {
                            IdPreparat = Convert.ToInt32(reader["IdPreparat"]),
                            Denumire = reader["Denumire"].ToString() ?? string.Empty,
                            Pret = Convert.ToDecimal(reader["Pret"]),
                            CantitatePortie = Convert.ToInt32(reader["CantitatePortiePreparat"]),
                            CantitateTotala = Convert.ToInt32(reader["CantitateTotala"])
                        }
                    };
                    meniu.Componente.Add(componenta);
                }
            }
            return meniuri;
        }
        public int AddMeniu(string denumire, int idCategorie, List<MeniuPreparatItem> componente)
        {
            using SqlConnection connection = DALHelper.Connection;
            SqlCommand command = new("AddMeniu", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@denumire", denumire));
            command.Parameters.Add(new SqlParameter("@idCategorie", idCategorie));
            DataTable table = new();
            table.Columns.Add("IdPreparat", typeof(int));
            table.Columns.Add("CantitatePortie", typeof(decimal));
            foreach (var c in componente)
                table.Rows.Add(c.IdPreparat, c.CantitatePortie);
            SqlParameter param = new("@componente", table);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.MeniuPreparatType";
            command.Parameters.Add(param);
            SqlParameter idOut = new("@idMeniu", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(idOut);
            connection.Open();
            command.ExecuteNonQuery();
            return (int)idOut.Value;
        }
        public int GetNrComenziClientInInterval(int idUtilizator, int zile)
        {
            using SqlConnection connection = DALHelper.Connection;
            SqlCommand command = new("GetNrComenziClientInInterval", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
            command.Parameters.Add(new SqlParameter("@zile", zile));
            connection.Open();
            object? result = command.ExecuteScalar();
            return result == null ? 0 : Convert.ToInt32(result);
        }
    }
}
