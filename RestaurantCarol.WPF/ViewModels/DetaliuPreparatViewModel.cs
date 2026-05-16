using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class DetaliuPreparatViewModel : ViewModelBase
    {
        private Preparat preparat;
        private PreparatBLL preparatBLL = new PreparatBLL();

        public string Denumire => preparat.Denumire;
        public string PretText => $"{preparat.Pret:F2} RON";
        public string CantitatePortieText => $"{preparat.CantitatePortie}g";
        public string? Descriere => preparat.Descriere;
        public bool AreDescriere => !string.IsNullOrWhiteSpace(Descriere);
        public string CalePoza => string.IsNullOrEmpty(preparat.PrimaCalePoza) ? "/Images/carol_logo.png" : preparat.PrimaCalePoza;
        public bool EsteDisponibil => preparat.EsteDisponibil;
        public bool PoateAdaugaInCos => UserSession.IsLoggedIn && UserSession.IsClient && EsteDisponibil;

        public ObservableCollection<Alergen> Alergeni { get; } = new();

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

        public string AdaugaText => $"Adauga in cos - {preparat.Pret * cantitate:F2} RON";

        public DetaliuPreparatViewModel(Preparat preparat)
        {
            this.preparat = preparat;
            foreach (var a in preparatBLL.GetAlergeniByPreparat(preparat.IdPreparat))
                Alergeni.Add(a);
        }

        public ICommand PlusCommand => new RelayCommand<object>(_ => Cantitate++);
        public ICommand MinusCommand => new RelayCommand<object>(_ => { if (Cantitate > 1) Cantitate--; });
        public ICommand AdaugaCommand => new RelayCommand<object>(_ =>
        {
            CartSession.AdaugaPreparat(preparat, Cantitate);
            InchideRequested?.Invoke();
        });
        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());

        public event Action? InchideRequested;
    }
}
