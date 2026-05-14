using System.Collections.ObjectModel;
using RestaurantCarol.Exceptions;

namespace RestaurantCarol.Layers
{
    public class ComandaBLL
    {
        private ComandaDAL comandaDAL = new ComandaDAL();

        private const decimal X_PROCENT_FIDEL = 10;
        private const decimal Y_VAL_MIN_FIDEL = 50;
        private const decimal Z_PROCENT_MARE = 15;
        private const decimal T_PRAG_COMANDA_MARE = 200;
        private const int W_MIN_COMENZI_FIDEL = 5;
        private const int A_PERIOADA_ZILE = 90;
        private const decimal B_PRAG_TRANSPORT_GRATUIT = 100;
        private const decimal C_COST_TRANSPORT = 15;
        private const int MINUTE_LIVRARE = 60;

        public class RezultatPlasareComanda
        {
            public string CodComanda { get; set; } = string.Empty;
            public DateTime OraEstimataLivrare { get; set; }
            public decimal CostMancare { get; set; }
            public decimal CostTransport { get; set; }
            public decimal Discount { get; set; }
            public decimal CostTotal => CostMancare + CostTransport - Discount;
            public int PuncteCastigate => (int)Math.Floor(CostTotal / 10);
        }

        public RezultatPlasareComanda PlaseazaComanda(int idUtilizator,
                                                     int idAdresaLivrare,
                                                     ObservableCollection<CartItem> iteme)
        {
            if (iteme == null || iteme.Count == 0)
            {
                throw new RestaurantException("Cosul este gol. Adauga produse inainte de a plasa comanda.");
            }

            if (idAdresaLivrare <= 0)
            {
                throw new RestaurantException("Trebuie sa selectezi o adresa de livrare.");
            }

            decimal costMancare = 0;
            foreach (var item in iteme)
            {
                costMancare += item.Subtotal;
            }

            decimal discount = CalculeazaDiscount(costMancare);

            decimal costMancareDupaDiscount = costMancare - discount;
            decimal costTransport = costMancareDupaDiscount >= B_PRAG_TRANSPORT_GRATUIT
                ? 0
                : C_COST_TRANSPORT;

            string codComanda = CodComandaGenerator.GenereazaCod();

            DateTime oraEstimataLivrare = DateTime.Now.AddMinutes(MINUTE_LIVRARE);

            try
            {
                int idComanda = comandaDAL.PlaseazaComanda(
                    idUtilizator,
                    idAdresaLivrare,
                    codComanda,
                    oraEstimataLivrare,
                    costMancare,
                    costTransport,
                    discount,
                    iteme);

                if (UserSession.CurrentUser != null)
                {
                    var utilizatorBLL = new AutentificareBLL();
                    UserSession.CurrentUser.Puncte = utilizatorBLL.GetPuncteByUtilizator(idUtilizator);
                }

                return new RezultatPlasareComanda
                {
                    CodComanda = codComanda,
                    OraEstimataLivrare = oraEstimataLivrare,
                    CostMancare = costMancare,
                    CostTransport = costTransport,
                    Discount = discount
                };
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 51000 || ex.Number == 51003)
            {
                throw new RestaurantException(ex.Message);
            }
        }

        private decimal CalculeazaDiscount(decimal costMancare)
        {
            decimal discountFidel = 0;
            decimal discountMare = 0;

            if (EsteClientFidel() && costMancare >= Y_VAL_MIN_FIDEL)
            {
                discountFidel = costMancare * X_PROCENT_FIDEL / 100;
            }

            if (costMancare >= T_PRAG_COMANDA_MARE)
            {
                discountMare = costMancare * Z_PROCENT_MARE / 100;
            }

            return Math.Max(discountFidel, discountMare);
        }

        private bool EsteClientFidel()
        {
            return false;
        }
    }
}