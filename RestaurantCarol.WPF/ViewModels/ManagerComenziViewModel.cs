using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class ManagerComenziViewModel : ViewModelBase
    {
        public event Action? InapoiRequested;

        private ComandaBLL comandaBLL = new ComandaBLL();
        private bool doarActive;

        public ObservableCollection<ComandaManagerItem> Comenzi { get; } = new();
        public List<StareComanda> StariDisponibile { get; }

        public string TitleText => doarActive ? "Comenzi Active" : "Toate Comenzile";

        public ManagerComenziViewModel(bool doarActive)
        {
            this.doarActive = doarActive;
            StariDisponibile = Enum.GetValues(typeof(StareComanda)).Cast<StareComanda>().ToList();
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                var lista = comandaBLL.GetComenziManager(doarActive);
                Comenzi.Clear();
                foreach (var c in lista)
                    Comenzi.Add(new ComandaManagerItem(c));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea comenzilor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand InapoiCommand => new RelayCommand<object>(_ => InapoiRequested?.Invoke());
        public ICommand AplicaStareCommand => new RelayCommand<ComandaManagerItem>(AplicaStare);

        private void AplicaStare(ComandaManagerItem item)
        {
            if (item == null) return;

            if (item.StareComanda == StareComanda.Livrata ||
                item.StareComanda == StareComanda.Anulata)
            {
                throw new RestaurantException(
                    "Aceasta comanda este intr-o stare finala si nu mai poate fi modificata.");
            }

            if (item.StareSelectata == item.StareComanda)
            {
                MessageBox.Show("Nu ai schimbat starea comenzii.",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            comandaBLL.UpdateStareComanda(item.IdComanda, item.StareSelectata);

            MessageBox.Show("Starea comenzii a fost actualizata cu succes!",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            IncarcaDate();
        }
    }
}