using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class ComandaManagerItem : ViewModelBase
    {
        public Comanda Comanda { get; }

        public int IdComanda => Comanda.IdComanda;
        public string CodComanda => Comanda.CodComanda;
        public DateTime DataComanda => Comanda.DataComanda;
        public decimal CostTotal => Comanda.CostTotal;
        public decimal CostMancare => Comanda.CostMancare;
        public decimal CostTransport => Comanda.CostTransport;
        public decimal Discount => Comanda.Discount;
        public DateTime? OraEstimataLivrare => Comanda.OraEstimataLivrare;
        public Utilizator? Utilizator => Comanda.Utilizator;
        public string AdresaLivrareCompleta => Comanda.AdresaLivrareCompleta;
        public List<ItemComanda> Items => Comanda.Items;

        public StareComanda StareComanda => Comanda.StareComanda;

        private StareComanda stareSelectata;
        public StareComanda StareSelectata
        {
            get => stareSelectata;
            set { stareSelectata = value; NotifyPropertyChanged(); }
        }

        public ComandaManagerItem(Comanda comanda)
        {
            Comanda = comanda;
            stareSelectata = comanda.StareComanda;
        }
    }
}