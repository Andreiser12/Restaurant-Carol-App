using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class StocViewModel : ViewModelBase
    {
        public event Action? InchideRequested;

        private PreparatBLL preparatBLL = new PreparatBLL();

        private List<(int IdPreparat, int CantitateOriginala)> snapshotOriginal = new();

        public ObservableCollection<Preparat> Preparate { get; } = new();

        public StocViewModel()
        {
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                var lista = preparatBLL.GetAllPreparate();

                Preparate.Clear();
                snapshotOriginal.Clear();

                foreach (var p in lista)
                {
                    Preparate.Add(p);
                    snapshotOriginal.Add((p.IdPreparat, p.CantitateTotala));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea produselor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SubmitCommand => new RelayCommand<object>(_ => Submit());
        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());

        private void Submit()
        {
            int modificate = 0;

            foreach (var preparat in Preparate)
            {
                var snap = snapshotOriginal.FirstOrDefault(s => s.IdPreparat == preparat.IdPreparat);
                if (snap.IdPreparat == 0) continue;

                if (snap.CantitateOriginala == preparat.CantitateTotala) continue;

                if (preparat.CantitateTotala < 0)
                    throw new RestaurantException(
                        $"Cantitatea pentru '{preparat.Denumire}' nu poate fi negativa.");

                preparatBLL.UpdateStoc(preparat.IdPreparat, preparat.CantitateTotala);
                modificate++;
            }

            if (modificate > 0)
            {
                MessageBox.Show($"Stocul a fost actualizat cu succes pentru {modificate} produse!",
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nu a fost modificata nicio cantitate.",
                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            InchideRequested?.Invoke();
        }
    }
}