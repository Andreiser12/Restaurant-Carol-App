using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AlergenSelectabil : ViewModelBase
    {
        public Alergen Alergen { get; }

        public int IdAlergen => Alergen.IdAlergen;
        public string Denumire => Alergen.Denumire;

        private bool isBifat;
        public bool IsBifat
        {
            get => isBifat;
            set { isBifat = value; NotifyPropertyChanged(); }
        }

        public AlergenSelectabil(Alergen alergen, bool isBifat = false)
        {
            Alergen = alergen;
            this.isBifat = isBifat;
        }
    }
}