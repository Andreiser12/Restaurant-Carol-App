using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AlergenFiltru : ViewModelBase
    {
        public Alergen Alergen { get; }

        public int IdAlergen => Alergen.IdAlergen;
        public string Denumire => Alergen.Denumire;

        private bool isBifat;
        public bool IsBifat
        {
            get => isBifat;
            set
            {
                isBifat = value;
                NotifyPropertyChanged();
                BifatSchimbat?.Invoke();
            }
        }

        public event Action? BifatSchimbat;

        public AlergenFiltru(Alergen alergen)
        {
            Alergen = alergen;
        }
    }
}