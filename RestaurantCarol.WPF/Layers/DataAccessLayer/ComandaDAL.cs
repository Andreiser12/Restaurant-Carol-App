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
    }
}