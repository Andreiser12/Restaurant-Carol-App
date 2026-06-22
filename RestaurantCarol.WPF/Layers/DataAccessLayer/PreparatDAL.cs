using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
namespace RestaurantCarol.Layers
{
    public class PreparatDAL
    {
        public ObservableCollection<Preparat> GetByCategorie(int idCategorie)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetPreparateByCategorie", connection);
                ObservableCollection<Preparat> result = [];
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idCategorie", idCategorie));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAlergeniByPreparat", connection);
                ObservableCollection<Alergen> result = [];
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idPreparat", idPreparat));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAllPreparate", connection);
                ObservableCollection<Preparat> result = new ObservableCollection<Preparat>();
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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
                            IdsAlergeni = ReadNullableString(reader, "IdsAlergeni")
                        });
                    }
                }
                return result;
            }
        }

        public ObservableCollection<Preparat> GetTopPopulare(int top = 3)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetTopPreparatePopulare", connection);
                ObservableCollection<Preparat> result = [];
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@top", top));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetAllAlergeni", connection);
                ObservableCollection<Alergen> result = [];
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("AddPreparat", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@denumire", preparat.Denumire));
                command.Parameters.Add(new SqlParameter("@pret", preparat.Pret));
                command.Parameters.Add(new SqlParameter("@cantitatePortie", preparat.CantitatePortie));
                command.Parameters.Add(new SqlParameter("@cantitateTotala", preparat.CantitateTotala));
                command.Parameters.Add(new SqlParameter("@descriere",
                    (object?)preparat.Descriere ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@calorii",
                    (object?)preparat.Calorii ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@grasimi",
                    (object?)preparat.Grasimi ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@carbohidrati",
                    (object?)preparat.Carbohidrati ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@proteine",
                    (object?)preparat.Proteine ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@sare",
                    (object?)preparat.Sare ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@idCategorie", preparat.IdCategorie));
                command.Parameters.Add(new SqlParameter("@caleFotografie",
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
                command.Parameters.Add(paramAlergeni);
                SqlParameter paramId = new("@idPreparat", SqlDbType.Int);
                paramId.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramId);
                connection.Open();
                command.ExecuteNonQuery();
                preparat.IdPreparat = (int)paramId.Value;
            }
        }

        public Preparat? GetById(int idPreparat)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetPreparatById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idPreparat", idPreparat));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Preparat
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
                        };
                    }
                    return null;
                }
            }
        }

        public bool CheckDenumireDuplicate(string denumire, int idExclude)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("CheckDenumirePreparatDuplicate", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@denumire", denumire));
                command.Parameters.Add(new SqlParameter("@idExclude", idExclude));
                connection.Open();
                object? result = command.ExecuteScalar();
                if (result == null) return false;
                return (int)result == 1;
            }
        }

        public void UpdatePreparat(Preparat preparat, List<int> idsAlergeni,
                                    string actiunePoza, string? caleFotografieNoua)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("UpdatePreparat", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idPreparat", preparat.IdPreparat));
                command.Parameters.Add(new SqlParameter("@denumire", preparat.Denumire));
                command.Parameters.Add(new SqlParameter("@pret", preparat.Pret));
                command.Parameters.Add(new SqlParameter("@cantitatePortie", preparat.CantitatePortie));
                command.Parameters.Add(new SqlParameter("@descriere",
                    (object?)preparat.Descriere ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@calorii",
                    (object?)preparat.Calorii ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@grasimi",
                    (object?)preparat.Grasimi ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@carbohidrati",
                    (object?)preparat.Carbohidrati ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@proteine",
                    (object?)preparat.Proteine ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@sare",
                    (object?)preparat.Sare ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@idCategorie", preparat.IdCategorie));
                command.Parameters.Add(new SqlParameter("@caleFotografie",
                    (object?)caleFotografieNoua ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("@actiunePoza", actiunePoza));
                DataTable alergeniTable = new DataTable();
                alergeniTable.Columns.Add("IdAlergen", typeof(int));
                foreach (int id in idsAlergeni)
                {
                    alergeniTable.Rows.Add(id);
                }
                SqlParameter paramAlergeni = new("@alergeni", alergeniTable);
                paramAlergeni.SqlDbType = SqlDbType.Structured;
                paramAlergeni.TypeName = "IdAlergenType";
                command.Parameters.Add(paramAlergeni);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStoc(int idPreparat, int cantitateNoua)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("UpdateStocPreparat", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idPreparat", idPreparat));
                command.Parameters.Add(new SqlParameter("@cantitateTotala", cantitateNoua));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public ObservableCollection<Preparat> GetPreparateStocRedus(int prag)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("GetPreparateStocRedus", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@prag", prag));
                ObservableCollection<Preparat> result = new();
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Preparat
                        {
                            IdPreparat = reader.GetInt32(reader.GetOrdinal("IdPreparat")),
                            Denumire = reader.GetString(reader.GetOrdinal("Denumire")),
                            CantitateTotala = Convert.ToInt32(reader["CantitateTotala"])
                        });
                    }
                }
                return result;
            }
        }

        public string? DeletePreparat(int idPreparat)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new("DeletePreparat", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@idPreparat", idPreparat));
                SqlParameter paramCalePoza = new("@calePozaDeStersOutput", SqlDbType.NVarChar, 500);
                paramCalePoza.Direction = ParameterDirection.Output;
                command.Parameters.Add(paramCalePoza);
                connection.Open();
                command.ExecuteNonQuery();
                if (paramCalePoza.Value == DBNull.Value) return null;
                return paramCalePoza.Value as string;
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