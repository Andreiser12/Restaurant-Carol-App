using System.Collections.ObjectModel;
using System.Data;
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
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("PlaseazaComanda", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@idUtilizator", idUtilizator));
                cmd.Parameters.Add(new SqlParameter("@idAdresaLivrare", idAdresaLivrare));
                cmd.Parameters.Add(new SqlParameter("@codComanda", codComanda));
                cmd.Parameters.Add(new SqlParameter("@oraEstimataLivrare", oraEstimataLivrare));
                cmd.Parameters.Add(new SqlParameter("@costMancare", costMancare));
                cmd.Parameters.Add(new SqlParameter("@costTransport", costTransport));
                cmd.Parameters.Add(new SqlParameter("@discount", discount));

                DataTable itemeTable = new DataTable();
                itemeTable.Columns.Add("IdPreparat", typeof(int));
                itemeTable.Columns.Add("Cantitate", typeof(int));

                foreach (var item in iteme)
                {
                    if (item.Preparat != null)
                    {
                        itemeTable.Rows.Add(item.Preparat.IdPreparat, item.Cantitate);
                    }
                }

                SqlParameter itemeParam = new SqlParameter("@itemeComanda", itemeTable);
                itemeParam.SqlDbType = SqlDbType.Structured;
                itemeParam.TypeName = "dbo.ItemComandaType";
                cmd.Parameters.Add(itemeParam);

                SqlParameter idParam = new("@idComanda", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(idParam);

                con.Open();
                cmd.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public ObservableCollection<Comanda> GetComenziManager(bool doarActive)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetComenziManager", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@doarActive", doarActive));

                ObservableCollection<Comanda> comenzi = new ObservableCollection<Comanda>();

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
                            StareComanda = (StareComanda)Enum.Parse(typeof(StareComanda), reader["Stare"].ToString() ?? "Inregistrata", true),
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

                    // Move to second result set for items
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int idComanda = Convert.ToInt32(reader["IdComanda"]);
                            var comanda = comenzi.FirstOrDefault(c => c.IdComanda == idComanda);
                            if (comanda != null)
                            {
                                comanda.Items.Add(new ItemComanda
                                {
                                    IdItemComanda = Convert.ToInt32(reader["IdItemComanda"]),
                                    IdComanda = idComanda,
                                    IdPreparat = reader["IdPreparat"] != DBNull.Value ? Convert.ToInt32(reader["IdPreparat"]) : null,
                                    Cantitate = Convert.ToInt32(reader["Cantitate"]),
                                    Preparat = new Preparat
                                    {
                                        IdPreparat = reader["IdPreparat"] != DBNull.Value ? Convert.ToInt32(reader["IdPreparat"]) : 0,
                                        Denumire = reader["DenumirePreparat"].ToString() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }

                return comenzi;
            }
        }

        public void UpdateStareComanda(int idComanda, StareComanda nouaStare)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("UpdateStareComanda", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idComanda", idComanda));
                cmd.Parameters.Add(new SqlParameter("@nouaStare", nouaStare.ToString()));

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}