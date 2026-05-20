using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;
using RestaurantCarol.Views.Navigation;

namespace RestaurantCarol.ViewModels
{
    public class HubViewModel : ViewModelBase
    {
        public event Action<Preparat>? DeschideDetaliuPreparatCerere;

        private PreparatBLL preparatBLL = new PreparatBLL();
        private AlergenBLL alergenBLL = new AlergenBLL();
        private IMeniuRestaurantNavigator? navigator;

        private ObservableCollection<Preparat> totalPreparate = new();

        public ObservableCollection<AlergenFiltru> AlergeniFiltru { get; } = new();
        public ObservableCollection<Preparat> Rezultate { get; } = new();

        private string textCautare = string.Empty;
        public string TextCautare
        {
            get => textCautare;
            set
            {
                textCautare = value;
                NotifyPropertyChanged();
                AplicaFiltre();
            }
        }

        private bool alergeniPanouDeschis;
        public bool AlergeniPanouDeschis
        {
            get => alergeniPanouDeschis;
            set
            {
                alergeniPanouDeschis = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(VizibilitateAlergeniPanel));
                NotifyPropertyChanged(nameof(TextButonAlergeni));
            }
        }

        public Visibility VizibilitateAlergeniPanel =>
            AlergeniPanouDeschis ? Visibility.Visible : Visibility.Collapsed;

        public string TextButonAlergeni =>
            AlergeniPanouDeschis ? "▲ Filtreaza alergeni" : "▼ Filtreaza alergeni";

        private bool searchActiv;
        public bool SearchActiv
        {
            get => searchActiv;
            set
            {
                searchActiv = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(VizibilitateOpresteCautare));
                NotifyPropertyChanged(nameof(VizibilitateCercuriNavigare));
                NotifyPropertyChanged(nameof(VizibilitateRezultate));
            }
        }

        public Visibility VizibilitateOpresteCautare =>
            SearchActiv ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VizibilitateCercuriNavigare =>
            SearchActiv ? Visibility.Collapsed : Visibility.Visible;

        public Visibility VizibilitateRezultate =>
            SearchActiv ? Visibility.Visible : Visibility.Collapsed;

        private string mesajGolText = string.Empty;
        public string MesajGolText
        {
            get => mesajGolText;
            set { mesajGolText = value; NotifyPropertyChanged(); }
        }

        private Visibility vizibilitateMesajGol = Visibility.Collapsed;
        public Visibility VizibilitateMesajGol
        {
            get => vizibilitateMesajGol;
            set { vizibilitateMesajGol = value; NotifyPropertyChanged(); }
        }

        private Visibility vizibilitateListaRezultate = Visibility.Visible;
        public Visibility VizibilitateListaRezultate
        {
            get => vizibilitateListaRezultate;
            set { vizibilitateListaRezultate = value; NotifyPropertyChanged(); }
        }

        public HubViewModel(IMeniuRestaurantNavigator navigator)
        {
            this.navigator = navigator;
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                totalPreparate = preparatBLL.GetAllPreparate();

                var alergeni = alergenBLL.GetAllAlergeni();
                AlergeniFiltru.Clear();
                foreach (var a in alergeni)
                {
                    var filtru = new AlergenFiltru(a);
                    filtru.BifatSchimbat += AplicaFiltre;
                    AlergeniFiltru.Add(filtru);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare date: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AplicaFiltre()
        {
            string text = TextCautare.Trim().ToLower();
            var alergeniDeEvitat = AlergeniFiltru.Where(a => a.IsBifat).Select(a => a.IdAlergen).ToList();

            bool esteActiv = !string.IsNullOrEmpty(text) || alergeniDeEvitat.Count > 0;
            SearchActiv = esteActiv;

            if (!esteActiv) return;

            var rezultate = totalPreparate.Where(p =>
            {
                bool potriveste = string.IsNullOrEmpty(text)
                    || p.Denumire.ToLower().Contains(text);
                if (!potriveste) return false;

                foreach (int idAlergen in alergeniDeEvitat)
                {
                    string cautare = $",{idAlergen},";
                    if (p.IdsAlergeni.Contains(cautare)) return false;
                }
                return true;
            }).ToList();

            Rezultate.Clear();
            foreach (var r in rezultate)
                Rezultate.Add(r);

            if (rezultate.Count == 0)
            {
                VizibilitateMesajGol = Visibility.Visible;
                VizibilitateListaRezultate = Visibility.Collapsed;

                if (!string.IsNullOrEmpty(text) && alergeniDeEvitat.Count > 0)
                    MesajGolText = $"Nu am gasit preparate cu '{TextCautare}' care sa nu contina alergenii selectati.";
                else if (!string.IsNullOrEmpty(text))
                    MesajGolText = $"Nu am gasit preparate cu '{TextCautare}'.";
                else
                    MesajGolText = "Nu am gasit preparate fara alergenii selectati.";
            }
            else
            {
                VizibilitateMesajGol = Visibility.Collapsed;
                VizibilitateListaRezultate = Visibility.Visible;
            }
        }

        public ICommand ToggleAlergeniCommand => new RelayCommand<object>(_ =>
            AlergeniPanouDeschis = !AlergeniPanouDeschis);

        public ICommand OpresteCautareaCommand => new RelayCommand<object>(_ =>
        {
            TextCautare = string.Empty;
            foreach (var a in AlergeniFiltru)
                a.IsBifat = false;
            AlergeniPanouDeschis = false;
        });

        public ICommand PopulareCommand => new RelayCommand<object>(_ =>
            navigator?.NavigateToListaPreparatePopulare());

        public ICommand MancareCommand => new RelayCommand<object>(_ =>
            navigator?.NavigateToListaCategorii(TipCategorie.Mancare, "Mancare", "/Images/categorie_mancare.png"));

        public ICommand BauturiCommand => new RelayCommand<object>(_ =>
            navigator?.NavigateToListaCategorii(TipCategorie.Bauturi, "Bauturi", "/Images/categorie_bauturi.png"));

        public ICommand PreparatCommand => new RelayCommand<Preparat>(preparat =>
            DeschideDetaliuPreparatCerere?.Invoke(preparat));
    }
}