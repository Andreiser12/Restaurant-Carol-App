using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class ComandaDAL
    {
        public int PlaseazaComanda(int idUtilizator, int idAdresaLivrare,
                            string codComanda,
                            DateTime oraEstimataLivrare,
                            decimal costMancare, decimal costTransport,
                            decimal discount,
                            ObservableCollection<CartItem> iteme)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("PlaseazaComanda", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                command.Parameters.Add(new SqlParameter("@idAdresaLivrare", idAdresaLivrare));
                command.Parameters.Add(new SqlParameter("@codComanda", codComanda));
                command.Parameters.Add(new SqlParameter("@oraEstimataLivrare", oraEstimataLivrare));
                command.Parameters.Add(new SqlParameter("@costMancare", costMancare));
                command.Parameters.Add(new SqlParameter("@costTransport", costTransport));
                command.Parameters.Add(new SqlParameter("@discount", discount));
                DataTable itemeTable = new DataTable();
                itemeTable.Columns.Add("IdPreparat", typeof(int));
                itemeTable.Columns.Add("IdMeniu", typeof(int));
                itemeTable.Columns.Add("Cantitate", typeof(int));
                foreach (var item in iteme)
                {
                    if (item.Preparat != null)
                        itemeTable.Rows.Add(item.Preparat.IdPreparat, DBNull.Value, item.Cantitate);
                    else if (item.Meniu != null)
                        itemeTable.Rows.Add(DBNull.Value, item.Meniu.IdMeniu, item.Cantitate);
                }
                SqlParameter itemeParam = new SqlParameter("@itemeComanda", itemeTable);
                itemeParam.SqlDbType = SqlDbType.Structured;
                itemeParam.TypeName = "dbo.ItemComandaType";
                command.Parameters.Add(itemeParam);
                SqlParameter idParam = new("@idComanda", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(idParam);
                connection.Open();
                command.ExecuteNonQuery();
                return (int)idParam.Value;
            }
        }

        public ObservableCollection<Comanda> GetComenziManager(bool doarActive)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetComenziManager", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@doarActive", SqlDbType.Bit) { Value = doarActive });
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                return LoadComenziFromReader(reader);
            }
        }

        public ObservableCollection<Comanda> GetComenziLivrator()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetComenziLivrator", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                return LoadComenziFromReader(reader);
            }
        }

        public ObservableCollection<Comanda> GetComenziClient(int idUtilizator, bool doarActive)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetComenziClient", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                command.Parameters.Add(new SqlParameter("@doarActive", SqlDbType.Bit) { Value = doarActive });
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                return LoadComenziFromReader(reader);
            }
        }

        private static ObservableCollection<Comanda> LoadComenziFromReader(SqlDataReader reader)
        {
            ObservableCollection<Comanda> comenzi = new ObservableCollection<Comanda>();
            while (reader.Read())
            {
                var comanda = new Comanda
                {
                    IdComanda = Convert.ToInt32(reader["IdComanda"]),
                    CodComanda = reader["CodComanda"].ToString() ?? string.Empty,
                    IdUtilizator = Convert.ToInt32(reader["IdUtilizator"]),
                    DataComanda = Convert.ToDateTime(reader["DataComanda"]),
                    OraEstimataLivrare = reader["OraEstimataLivrare"] != DBNull.Value ? Convert.ToDateTime(reader["OraEstimataLivrare"]) : null,
                    CostMancare = Convert.ToDecimal(reader["CostMancare"]),
                    CostTransport = Convert.ToDecimal(reader["CostTransport"]),
                    Discount = Convert.ToDecimal(reader["Discount"]),
                    StareComanda = GetStareFromDbString(reader["Stare"].ToString() ?? "inregistrata"),
                    AdresaLivrareCompleta = reader["AdresaLivrareCompleta"].ToString() ?? string.Empty,
                    Utilizator = new Utilizator
                    {
                        Nume = reader["NumeClient"].ToString() ?? string.Empty,
                        Prenume = reader["PrenumeClient"].ToString() ?? string.Empty,
                        Telefon = reader["TelefonClient"] != DBNull.Value ? reader["TelefonClient"].ToString() : null
                    }
                };
                comenzi.Add(comanda);
            }
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    int idComanda = Convert.ToInt32(reader["IdComanda"]);
                    var comanda = comenzi.FirstOrDefault(c => c.IdComanda == idComanda);
                    if (comanda != null)
                    {
                        int? idPrep = reader["IdPreparat"] != DBNull.Value ? Convert.ToInt32(reader["IdPreparat"]) : null;
                        int? idMen = reader["IdMeniu"] != DBNull.Value ? Convert.ToInt32(reader["IdMeniu"]) : null;
                        string denumire = idMen.HasValue
                            ? (reader["DenumireMeniu"].ToString() ?? "Meniu")
                            : (reader["DenumirePreparat"].ToString() ?? string.Empty);
                        var itemComanda = new ItemComanda
                        {
                            IdItemComanda = Convert.ToInt32(reader["IdItemComanda"]),
                            IdComanda = idComanda,
                            IdPreparat = idPrep,
                            IdMeniu = idMen,
                            Cantitate = Convert.ToInt32(reader["Cantitate"])
                        };
                        if (idMen.HasValue)
                            itemComanda.Meniu = new Meniu { IdMeniu = idMen.Value, Denumire = denumire };
                        else
                            itemComanda.Preparat = new Preparat
                            {
                                IdPreparat = idPrep ?? 0,
                                Denumire = denumire
                            };
                        comanda.Items.Add(itemComanda);
                    }
                }
            }
            return comenzi;
        }

        public void UpdateStareComanda(int idComanda, StareComanda nouaStare)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("UpdateStareComanda", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idComanda", idComanda));
                command.Parameters.Add(new SqlParameter("@nouaStare", GetStareDbString(nouaStare)));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static string GetStareDbString(StareComanda stare)
        {
            return stare switch
            {
                StareComanda.Inregistrata => "inregistrata",
                StareComanda.SePregateste => "se pregateste",
                StareComanda.APlecatLaClient => "a plecat la client",
                StareComanda.Livrata => "livrata",
                StareComanda.Anulata => "anulata",
                _ => "inregistrata"
            };
        }

        public int GetUrmatorulNumarComanda()
        {
            using SqlConnection connection = DALHelper.Connection;
            SqlCommand command = new(
                "SELECT ISNULL(MAX(IdComanda), 0) + 1 FROM Comanda", connection);
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private static StareComanda GetStareFromDbString(string stareDb)
        {
            return stareDb?.ToLower() switch
            {
                "inregistrata" => StareComanda.Inregistrata,
                "se pregateste" => StareComanda.SePregateste,
                "a plecat la client" => StareComanda.APlecatLaClient,
                "livrata" => StareComanda.Livrata,
                "anulata" => StareComanda.Anulata,
                _ => StareComanda.Inregistrata
            };
        }
    }
}