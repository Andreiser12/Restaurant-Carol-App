using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class LivratorComenziViewModel : ViewModelBase
    {
        public event Action? InapoiRequested;

        private ComandaBLL comandaBLL = new ComandaBLL();

        public ObservableCollection<Comanda> Comenzi { get; } = new();

        private Comanda? selectata;
        public Comanda? Selectata
        {
            get => selectata;
            set
            {
                selectata = value;
                NotifyPropertyChanged();
                LivratorSession.ComandaSelectata = value;
            }
        }

        private Visibility vizibilitateEmpty = Visibility.Collapsed;
        public Visibility VizibilitateEmpty
        {
            get => vizibilitateEmpty;
            set { vizibilitateEmpty = value; NotifyPropertyChanged(); }
        }

        public LivratorComenziViewModel()
        {
            IncarcaComenzi();
        }

        private void IncarcaComenzi()
        {
            try
            {
                int? idDeRestaurat = LivratorSession.ComandaSelectata?.IdComanda;

                var lista = comandaBLL.GetComenziLivrator();

                Comenzi.Clear();
                foreach (var c in lista)
                    Comenzi.Add(c);

                VizibilitateEmpty = Comenzi.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                if (idDeRestaurat.HasValue)
                {
                    var match = Comenzi.FirstOrDefault(c => c.IdComanda == idDeRestaurat.Value);
                    if (match != null)
                    {
                        Selectata = match;
                        LivratorSession.ComandaSelectata = match;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea comenzilor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand InapoiCommand => new RelayCommand<object>(_ => InapoiRequested?.Invoke());
    }
}