using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;
namespace RestaurantCarol.ViewModels
{
    public class StareComandaDetaliiViewModel : ViewModelBase
    {
        private Comanda comanda;
        private ComandaBLL comandaBLL = new ComandaBLL();
        public string Titlu => $"Comanda #{comanda.CodComanda}";
        public string DataText => $"Data: {comanda.DataComanda:dd.MM.yyyy HH:mm}";
        public string OraText => comanda.OraEstimataLivrare.HasValue
            ? $"Livrare estimata: {comanda.OraEstimataLivrare:HH:mm}" : "";
        public string TotalText => $"Total: {comanda.CostTotal:F2} RON";
        public IEnumerable<string> LiniiProduse => comanda.Items.Select(i =>
            $"{i.Cantitate} x {(i.Meniu?.Denumire ?? i.Preparat?.Denumire ?? "Produs")}");
        public bool EsteAnulata => comanda.StareComanda == StareComanda.Anulata;
        public bool PoateAnula => !StareComandaHelper.EsteStareFinala(comanda.StareComanda);
        public bool PasInregistrata => StareComandaHelper.EstePasAtins(comanda.StareComanda, StareComanda.Inregistrata);
        public bool PasSePregateste => StareComandaHelper.EstePasAtins(comanda.StareComanda, StareComanda.SePregateste);
        public bool PasAPlecat => StareComandaHelper.EstePasAtins(comanda.StareComanda, StareComanda.APlecatLaClient);
        public bool PasLivrata => StareComandaHelper.EstePasAtins(comanda.StareComanda, StareComanda.Livrata);
        public StareComandaDetaliiViewModel(Comanda comanda)
        {
            this.comanda = comanda;
        }
        public ICommand AnuleazaCommand => new RelayCommand<object>(_ =>
        {
            if (UserSession.CurrentUser == null) return;
            if (MessageBox.Show("Sigur vrei sa anulezi aceasta comanda?", "Confirmare",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            comandaBLL.AnuleazaComanda(comanda.IdComanda, UserSession.CurrentUser.IdUtilizator);
            Reincarca();
            ComandaActualizata?.Invoke();
            MessageBox.Show("Comanda a fost anulata.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        });
        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());
        public event Action? InchideRequested;
        public event Action? ComandaActualizata;
        private void Reincarca()
        {
            if (UserSession.CurrentUser == null) return;
            var actualizata = comandaBLL.GetComenziClient(UserSession.CurrentUser.IdUtilizator)
                .FirstOrDefault(c => c.IdComanda == comanda.IdComanda);
            if (actualizata != null)
                comanda = actualizata;
            NotifyPropertyChanged(nameof(Titlu));
            NotifyPropertyChanged(nameof(DataText));
            NotifyPropertyChanged(nameof(OraText));
            NotifyPropertyChanged(nameof(TotalText));
            NotifyPropertyChanged(nameof(LiniiProduse));
            NotifyPropertyChanged(nameof(EsteAnulata));
            NotifyPropertyChanged(nameof(PoateAnula));
            NotifyPropertyChanged(nameof(PasInregistrata));
            NotifyPropertyChanged(nameof(PasSePregateste));
            NotifyPropertyChanged(nameof(PasAPlecat));
            NotifyPropertyChanged(nameof(PasLivrata));
        }
    }
}
