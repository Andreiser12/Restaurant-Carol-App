using RestaurantCarol.Commands;
using RestaurantCarol.Layers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace RestaurantCarol.ViewModels
{
    public class StareComandaListViewModel : ViewModelBase
    {
        private ComandaBLL comandaBLL = new ComandaBLL();
        public ObservableCollection<Comanda> Comenzi { get; } = new();
        public bool EsteGol { get; private set; }
        public Visibility VizibilitateEmpty => EsteGol ? Visibility.Visible : Visibility.Collapsed;

        private Comanda? selectata;
        public Comanda? Selectata
        {
            get => selectata;
            set { selectata = value; NotifyPropertyChanged(); }
        }

        public StareComandaListViewModel()
        {
            Incarca();
        }

        public void Incarca()
        {
            Comenzi.Clear();
            if (UserSession.CurrentUser == null) return;
            foreach (var c in comandaBLL.GetComenziClient(UserSession.CurrentUser.IdUtilizator))
                Comenzi.Add(c);
            EsteGol = Comenzi.Count == 0;
            NotifyPropertyChanged(nameof(EsteGol));
            NotifyPropertyChanged(nameof(VizibilitateEmpty));
        }

        public ICommand DeschideSelectataCommand => new RelayCommand<object>(_ =>
        {
            if (Selectata != null)
                DeschideDetalii?.Invoke(Selectata);
        });

        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());
        public event Action<Comanda>? DeschideDetalii;
        public event Action? InchideRequested;
    }
}
