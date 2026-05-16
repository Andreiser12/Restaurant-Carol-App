using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;
using RestaurantCarol.Views.Navigation;

namespace RestaurantCarol.ViewModels
{
    public class ListaPreparateViewModel : ViewModelBase
    {
        private CatalogBLL catalogBLL = new CatalogBLL();

        public ObservableCollection<CatalogItem> Produse { get; } = new();

        public string Titlu { get; }
        public string CaleLogo { get; }

        private readonly IMeniuRestaurantNavigator? navigator;
        private readonly Categorie? categorie;
        private readonly TipCategorie? tipParinte;
        public ModListaPreparate Mod { get; }

        public ListaPreparateViewModel(
            IMeniuRestaurantNavigator? navigator,
            Categorie categorie,
            TipCategorie tipParinte,
            ModListaPreparate mod)
        {
            this.navigator = navigator;
            this.categorie = categorie;
            this.tipParinte = tipParinte;
            Mod = mod;
            Titlu = categorie.Denumire;
            CaleLogo = "/Images/carol_logo.png";
            IncarcaProduse();
        }

        public ListaPreparateViewModel(
            IMeniuRestaurantNavigator? navigator,
            ObservableCollection<Preparat> preparatePopulare,
            string titlu,
            string caleLogo)
        {
            this.navigator = navigator;
            Mod = ModListaPreparate.Browse;
            Titlu = titlu;
            CaleLogo = caleLogo;
            foreach (var p in preparatePopulare)
                Produse.Add(CatalogItem.DinPreparat(p));
        }

        public void IncarcaProduse()
        {
            if (categorie == null) return;
            Produse.Clear();
            foreach (var item in catalogBLL.GetByCategorie(categorie.IdCategorie))
                Produse.Add(item);
        }

        public ICommand InapoiCommand => new RelayCommand<object>(_ =>
        {
            if (Mod == ModListaPreparate.Browse)
            {
                if (tipParinte.HasValue)
                {
                    string titlu = tipParinte.Value == TipCategorie.Mancare ? "Mancare" : "Bauturi";
                    string img = tipParinte.Value == TipCategorie.Mancare
                        ? "/Images/categorie_mancare.jpg" : "/Images/categorie_bauturi.jpg";
                    navigator?.NavigateToListaCategorii(tipParinte.Value, titlu, img);
                }
                else
                    navigator?.NavigateToHub();
            }
            else
                InapoiEditRequested?.Invoke();
        });

        public ICommand SelecteazaProdusCommand => new RelayCommand<CatalogItem>(item =>
        {
            if (item == null) return;
            if (Mod == ModListaPreparate.Browse)
                DeschideDetaliiBrowse?.Invoke(item);
            else
                DeschideEdit?.Invoke(item);
        });

        public event Action? InapoiEditRequested;
        public event Action<CatalogItem>? DeschideDetaliiBrowse;
        public event Action<CatalogItem>? DeschideEdit;
    }
}
