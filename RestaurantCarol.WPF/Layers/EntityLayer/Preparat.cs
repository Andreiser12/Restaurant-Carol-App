namespace RestaurantCarol.Layers
{
    public class Preparat : BasePropertyChanged
    {
        private int idPreparat;
        public int IdPreparat
        {
            get => idPreparat;
            set { idPreparat = value; NotifyPropertyChanged(); }
        }

        private string denumire = string.Empty;
        public string Denumire
        {
            get => denumire;
            set { denumire = value; NotifyPropertyChanged(); }
        }

        private decimal pret;
        public decimal Pret
        {
            get => pret;
            set { pret = value; NotifyPropertyChanged(); }
        }

        private int cantitatePortie;
        public int CantitatePortie
        {
            get => cantitatePortie;
            set { cantitatePortie = value; NotifyPropertyChanged(); }
        }

        private int cantitateTotala;
        public int CantitateTotala
        {
            get => cantitateTotala;
            set { cantitateTotala = value; NotifyPropertyChanged(); }
        }

        private string? descriere;
        public string? Descriere
        {
            get => descriere;
            set { descriere = value; NotifyPropertyChanged(); }
        }

        private int? calorii;
        public int? Calorii
        {
            get => calorii;
            set { calorii = value; NotifyPropertyChanged(); }
        }

        private decimal? grasimi;
        public decimal? Grasimi
        {
            get => grasimi;
            set { grasimi = value; NotifyPropertyChanged(); }
        }

        private decimal? carbohidrati;
        public decimal? Carbohidrati
        {
            get => carbohidrati;
            set { carbohidrati = value; NotifyPropertyChanged(); }
        }

        private decimal? proteine;
        public decimal? Proteine
        {
            get => proteine;
            set { proteine = value; NotifyPropertyChanged(); }
        }

        private decimal? sare;
        public decimal? Sare
        {
            get => sare;
            set { sare = value; NotifyPropertyChanged(); }
        }

        private int idCategorie;
        public int IdCategorie
        {
            get => idCategorie;
            set { idCategorie = value; NotifyPropertyChanged(); }
        }

        private string? primaCalePoza;
        public string? PrimaCalePoza
        {
            get => primaCalePoza;
            set { primaCalePoza = value; NotifyPropertyChanged(); }
        }

        private string idsAlergeni = ",";
        public string IdsAlergeni
        {
            get => idsAlergeni;
            set { idsAlergeni = value; NotifyPropertyChanged(); }
        }


    }
}