using System.Collections.ObjectModel;
using System.Linq;
using RestaurantCarol.Exceptions;

namespace RestaurantCarol.Layers
{
    public class ComandaBLL
    {
        private ComandaDAL comandaDAL = new ComandaDAL();
        private MeniuDAL meniuDAL = new MeniuDAL();

        private const int MINUTE_LIVRARE = 60;

        public class RezultatPlasareComanda
        {
            public int IdComanda { get; set; }
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
            decimal costTransport = costMancareDupaDiscount >= AppSettingsHelper.PragTransportGratuit
                ? 0
                : AppSettingsHelper.CostTransport;

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
                    IdComanda = idComanda,
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

            if (EsteClientFidel())
            {
                discountFidel = costMancare * AppSettingsHelper.ProcentDiscount / 100;
            }

            if (costMancare >= AppSettingsHelper.PragDiscountSuma)
            {
                discountMare = costMancare * AppSettingsHelper.ProcentDiscount / 100;
            }

            return Math.Max(discountFidel, discountMare);
        }

        private bool EsteClientFidel()
        {
            if (UserSession.CurrentUser == null) return false;

            int nr = meniuDAL.GetNrComenziClientInInterval(
                UserSession.CurrentUser.IdUtilizator,
                AppSettingsHelper.IntervalZileDiscount);

            return nr >= AppSettingsHelper.NrComenziDiscount;
        }

        public ObservableCollection<Comanda> GetComenziManager(bool doarActive)
        {
            var comenzi = comandaDAL.GetComenziManager(doarActive);

            if (!doarActive)
                return comenzi;

            return new ObservableCollection<Comanda>(
                comenzi.Where(c =>
                    c.StareComanda != StareComanda.Livrata &&
                    c.StareComanda != StareComanda.Anulata));
        }

        public void UpdateStareComanda(int idComanda, StareComanda nouaStare)
        {
            if (idComanda <= 0)
                throw new RestaurantException("Comanda invalida.");

            comandaDAL.UpdateStareComanda(idComanda, nouaStare);
        }

        public ObservableCollection<Comanda> GetComenziLivrator()
        {
            return comandaDAL.GetComenziLivrator();
        }

        public void ConfirmaLivrare(int idComanda)
        {
            if (idComanda <= 0)
                throw new RestaurantException("Comanda invalida.");

            var comenzi = comandaDAL.GetComenziLivrator();
            var comanda = comenzi.FirstOrDefault(c => c.IdComanda == idComanda);

            if (comanda == null)
                throw new RestaurantException("Comanda nu este disponibila pentru livrare.");

            if (comanda.StareComanda != StareComanda.APlecatLaClient)
                throw new RestaurantException("Comanda nu este in starea „a plecat la client”.");

            comandaDAL.UpdateStareComanda(idComanda, StareComanda.Livrata);
        }

        public ObservableCollection<Comanda> GetComenziClient(int idUtilizator, bool doarActive = false)
        {
            if (idUtilizator <= 0)
                throw new RestaurantException("Utilizator invalid.");

            return comandaDAL.GetComenziClient(idUtilizator, doarActive);
        }

        public Comanda? GetUltimaComandaActivaClient(int idUtilizator)
        {
            var comenzi = GetComenziClient(idUtilizator, doarActive: true);
            return comenzi.FirstOrDefault();
        }

        public void AnuleazaComanda(int idComanda, int idUtilizator)
        {
            if (idComanda <= 0)
                throw new RestaurantException("Comanda invalida.");

            var comenzi = comandaDAL.GetComenziClient(idUtilizator, doarActive: true);
            var comanda = comenzi.FirstOrDefault(c => c.IdComanda == idComanda);

            if (comanda == null)
                throw new RestaurantException("Comanda nu poate fi anulata.");

            if (StareComandaHelper.EsteStareFinala(comanda.StareComanda))
                throw new RestaurantException("Comanda este deja intr-o stare finala.");

            comandaDAL.UpdateStareComanda(idComanda, StareComanda.Anulata);
        }
    }
}