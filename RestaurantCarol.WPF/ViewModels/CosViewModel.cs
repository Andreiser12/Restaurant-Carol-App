using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;
using RestaurantCarol.Views.Navigation;
namespace RestaurantCarol.ViewModels
{
    public class CosViewModel : ViewModelBase
    {
        private readonly IMeniuRestaurantNavigator? navigator;
        private AdresaBLL adresaBLL = new AdresaBLL();
        private ComandaBLL comandaBLL = new ComandaBLL();
        public ObservableCollection<CartItem> Items => CartSession.Items;
        public ObservableCollection<Adresa> Adrese { get; } = new();
        private Adresa? adresaSelectata;
        public Adresa? AdresaSelectata
        {
            get => adresaSelectata;
            set
            {
                adresaSelectata = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(PoatePlasa));
            }
        }
        public decimal Total => CartSession.CostTotal;
        public bool EsteGol => CartSession.EsteGol;
        public bool PoatePlasa => !EsteGol && AdresaSelectata != null;
        public CosViewModel(IMeniuRestaurantNavigator? navigator = null)
        {
            this.navigator = navigator;
            CartSession.CartChanged += OnCartChanged;
            IncarcaAdrese();
        }
        private void OnCartChanged()
        {
            NotifyPropertyChanged(nameof(Total));
            NotifyPropertyChanged(nameof(EsteGol));
            NotifyPropertyChanged(nameof(PoatePlasa));
        }
        public void IncarcaAdrese()
        {
            Adrese.Clear();
            if (UserSession.CurrentUser == null) return;
            var lista = adresaBLL.GetByUtilizator(UserSession.CurrentUser.IdUtilizator);
            foreach (var a in lista)
                Adrese.Add(a);
            AdresaSelectata = Adrese.FirstOrDefault(a => a.EsteImplicita) ?? Adrese.FirstOrDefault();
        }
        public ICommand PlusCommand => new RelayCommand<CartItem>(item =>
            CartSession.ModificaCantitate(item, item.Cantitate + 1));
        public ICommand MinusCommand => new RelayCommand<CartItem>(item =>
            CartSession.ModificaCantitate(item, item.Cantitate - 1));
        public ICommand StergeCommand => new RelayCommand<CartItem>(item =>
        {
            if (MessageBox.Show($"Sigur vrei sa stergi {item.Denumire} din cos?",
                    "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                CartSession.Sterge(item);
        });
        public ICommand PlaseazaCommand => new RelayCommand<object>(_ => PlaseazaComanda());
        public ICommand InchideCommand => new RelayCommand<object>(_ =>
            InchideRequested?.Invoke());
        public event Action? InchideRequested;
        public event Action? ComandaPlasata;
        private void PlaseazaComanda()
        {
            if (!UserSession.IsLoggedIn || UserSession.CurrentUser == null)
                throw new RestaurantException("Trebuie sa fii logat pentru a plasa o comanda.");
            if (AdresaSelectata == null)
                throw new RestaurantException("Selecteaza o adresa de livrare.");
            var confirm = MessageBox.Show(
                $"Sigur vrei sa plasezi comanda?\n\nTotal mancare: {CartSession.CostTotal:F2} RON\nAdresa: {AdresaSelectata.AdresaText}",
                "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;
            var rezultat = comandaBLL.PlaseazaComanda(
                UserSession.CurrentUser.IdUtilizator,
                AdresaSelectata.IdAdresa,
                CartSession.Items);
            MessageBox.Show($"Comanda #{rezultat.CodComanda} plasata. Total: {rezultat.CostTotal:F2} RON",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            navigator?.ActualizeazaStareComanda(rezultat);
            CartSession.Goleste();
            ComandaPlasata?.Invoke();
            InchideRequested?.Invoke();
        }
        public void Detach()
        {
            CartSession.CartChanged -= OnCartChanged;
        }
    }
}
