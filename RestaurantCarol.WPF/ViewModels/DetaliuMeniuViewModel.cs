using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class DetaliuMeniuViewModel : ViewModelBase
    {
        private Meniu meniu;

        public string Denumire => meniu.Denumire;
        public string PretText => $"{meniu.Pret:F2} RON";
        public string GramajeText => meniu.GramajeAfisate;
        public string CalePoza => Meniu.CalePozaImplicita;
        public bool EsteDisponibil => meniu.EsteDisponibil;
        public bool PoateAdaugaInCos => UserSession.IsLoggedIn && UserSession.IsClient && EsteDisponibil;

        private int cantitate = 1;
        public int Cantitate
        {
            get => cantitate;
            set
            {
                cantitate = Math.Max(1, value);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AdaugaText));
            }
        }

        public string AdaugaText => $"Adauga in cos - {meniu.Pret * cantitate:F2} RON";

        public DetaliuMeniuViewModel(Meniu meniu)
        {
            this.meniu = meniu;
        }

        public ICommand PlusCommand => new RelayCommand<object>(_ => Cantitate++);
        public ICommand MinusCommand => new RelayCommand<object>(_ => { if (Cantitate > 1) Cantitate--; });
        public ICommand AdaugaCommand => new RelayCommand<object>(_ =>
        {
            CartSession.AdaugaMeniu(meniu, Cantitate);
            InchideRequested?.Invoke();
        });
        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());

        public event Action? InchideRequested;
    }
}
