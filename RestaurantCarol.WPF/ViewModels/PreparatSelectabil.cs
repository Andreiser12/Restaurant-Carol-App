using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class PreparatSelectabil : ViewModelBase
    {
        public Preparat Preparat { get; }

        public int IdPreparat => Preparat.IdPreparat;
        public string Denumire => Preparat.Denumire;
        public int CantitatePortie => Preparat.CantitatePortie;

        private bool isBifat;
        public bool IsBifat
        {
            get => isBifat;
            set { isBifat = value; NotifyPropertyChanged(); }
        }

        public PreparatSelectabil(Preparat preparat)
        {
            Preparat = preparat;
        }
    }
}