using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RestaurantCarol.Layers
{
    public class PreparatDAL
    {
        public ObservableCollection<Preparat> GetByCategorie(int idCategorie)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetPreparateByCategorie", con);
                ObservableCollection<Preparat> result = [];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idCategorie", idCategorie));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Preparat
                        {
                            IdPreparat = reader.GetInt32(reader.GetOrdinal("IdPreparat")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire")),
                            Pret = reader.GetDecimal(reader.GetOrdinal("Pret")),
                            CantitatePortie = (int)reader.GetDecimal(reader.GetOrdinal("CantitatePortie")),
                            CantitateTotala = (int)reader.GetDecimal(reader.GetOrdinal("CantitateTotala")),
                            Descriere = ReadNullableString(reader, "Descriere"),
                            Calorii = ReadNullableInt(reader, "Calorii"),
                            Grasimi = ReadNullableDecimal(reader, "Grasimi"),
                            Carbohidrati = ReadNullableDecimal(reader, "Carbohidrati"),
                            Proteine = ReadNullableDecimal(reader, "Proteine"),
                            Sare = ReadNullableDecimal(reader, "Sare"),
                            IdCategorie = reader.GetInt32(reader.GetOrdinal("IdCategorie")),
                            PrimaCalePoza = ReadNullableString(reader, "PrimaCalePoza")
                        });
                    }
                }
                return result;
            }
        }

        public ObservableCollection<Alergen> GetAlergeniByPreparat(int idPreparat)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAlergeniByPreparat", con);
                ObservableCollection<Alergen> result = [];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idPreparat", idPreparat));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Alergen
                        {
                            IdAlergen = reader.GetInt32(reader.GetOrdinal("IdAlergen")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire"))
                        });
                    }
                }
                return result;
            }
        }

        public ObservableCollection<Preparat> GetAllPreparate()
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAllPreparate", con);
                ObservableCollection<Preparat> result = [];
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Preparat
                        {
                            IdPreparat = reader.GetInt32(reader.GetOrdinal("IdPreparat")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire")),
                            Pret = reader.GetDecimal(reader.GetOrdinal("Pret")),
                            CantitatePortie = (int)reader.GetDecimal(reader.GetOrdinal("CantitatePortie")),
                            CantitateTotala = (int)reader.GetDecimal(reader.GetOrdinal("CantitateTotala")),
                            Descriere = ReadNullableString(reader, "Descriere"),
                            Calorii = ReadNullableInt(reader, "Calorii"),
                            Grasimi = ReadNullableDecimal(reader, "Grasimi"),
                            Carbohidrati = ReadNullableDecimal(reader, "Carbohidrati"),
                            Proteine = ReadNullableDecimal(reader, "Proteine"),
                            Sare = ReadNullableDecimal(reader, "Sare"),
                            IdCategorie = reader.GetInt32(reader.GetOrdinal("IdCategorie")),
                            PrimaCalePoza = ReadNullableString(reader, "PrimaCalePoza"),
                            IdsAlergeni = reader.GetString(reader.GetOrdinal("IdsAlergeni"))
                        });
                    }
                }
                return result;
            }
        }

        public ObservableCollection<Preparat> GetTopPopulare(int top = 3)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetTopPreparatePopulare", con);
                ObservableCollection<Preparat> result = [];
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@top", top));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Preparat
                        {
                            IdPreparat = reader.GetInt32(reader.GetOrdinal("IdPreparat")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire")),
                            Pret = reader.GetDecimal(reader.GetOrdinal("Pret")),
                            CantitatePortie = (int)reader.GetDecimal(reader.GetOrdinal("CantitatePortie")),
                            CantitateTotala = (int)reader.GetDecimal(reader.GetOrdinal("CantitateTotala")),
                            Descriere = ReadNullableString(reader, "Descriere"),
                            Calorii = ReadNullableInt(reader, "Calorii"),
                            Grasimi = ReadNullableDecimal(reader, "Grasimi"),
                            Carbohidrati = ReadNullableDecimal(reader, "Carbohidrati"),
                            Proteine = ReadNullableDecimal(reader, "Proteine"),
                            Sare = ReadNullableDecimal(reader, "Sare"),
                            IdCategorie = reader.GetInt32(reader.GetOrdinal("IdCategorie")),
                            PrimaCalePoza = ReadNullableString(reader, "PrimaCalePoza")
                        });
                    }
                }
                return result;
            }
        }

        public ObservableCollection<Alergen> GetAllAlergeni()
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("GetAllAlergeni", con);
                ObservableCollection<Alergen> result = [];
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Alergen
                        {
                            IdAlergen = reader.GetInt32(reader.GetOrdinal("IdAlergen")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire"))
                        });
                    }
                }
                return result;
            }
        }

        public void AddPreparat(Preparat preparat, List<int> idsAlergeni, string? caleFotografie)
        {
            using (SqlConnection con = DALHelper.Connection)
            {
                SqlCommand cmd = new("AddPreparat", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@denumire", preparat.Denumire));
                cmd.Parameters.Add(new SqlParameter("@pret", preparat.Pret));
                cmd.Parameters.Add(new SqlParameter("@cantitatePortie", preparat.CantitatePortie));
                cmd.Parameters.Add(new SqlParameter("@cantitateTotala", preparat.CantitateTotala));
                cmd.Parameters.Add(new SqlParameter("@descriere",
                    (object?)preparat.Descriere ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@calorii",
                    (object?)preparat.Calorii ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@grasimi",
                    (object?)preparat.Grasimi ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@carbohidrati",
                    (object?)preparat.Carbohidrati ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@proteine",
                    (object?)preparat.Proteine ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@sare",
                    (object?)preparat.Sare ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@idCategorie", preparat.IdCategorie));
                cmd.Parameters.Add(new SqlParameter("@caleFotografie",
                    (object?)caleFotografie ?? DBNull.Value));

                DataTable alergeniTable = new DataTable();
                alergeniTable.Columns.Add("IdAlergen", typeof(int));
                foreach (int id in idsAlergeni)
                {
                    alergeniTable.Rows.Add(id);
                }

                SqlParameter paramAlergeni = new("@alergeni", alergeniTable);
                paramAlergeni.SqlDbType = SqlDbType.Structured;
                paramAlergeni.TypeName = "IdAlergenType";
                cmd.Parameters.Add(paramAlergeni);

                SqlParameter paramId = new("@idPreparat", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();

                preparat.IdPreparat = (int)paramId.Value;
            }
        }

        private string? ReadNullableString(SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return reader.IsDBNull(idx) ? null : reader.GetString(idx);
        }

        private int? ReadNullableInt(SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return reader.IsDBNull(idx) ? null : reader.GetInt32(idx);
        }

        private decimal? ReadNullableDecimal(SqlDataReader reader, string columnName)
        {
            int idx = reader.GetOrdinal(columnName);
            return reader.IsDBNull(idx) ? null : reader.GetDecimal(idx);
        }
    }
}