using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
using RestaurantCarol.Views.Navigation;
namespace RestaurantCarol.Views
{
    public partial class ListaPreparateUserControl : UserControl
    {
        private ListaPreparateViewModel? viewModel;
        private IMeniuRestaurantNavigator? navigator;
        private AngajatHubView? parentViewBucatar;

        public ListaPreparateUserControl()
        {
            InitializeComponent();
        }

        public ListaPreparateUserControl(IMeniuRestaurantNavigator parent, Categorie categorie,
            TipCategorie tipParinte) : this()
        {
            navigator = parent;
            viewModel = new ListaPreparateViewModel(parent, categorie, tipParinte, ModListaPreparate.Browse);
            DataContext = viewModel;
            ConfigureazaVmEvents();
            ConfigureazaUI(viewModel.Titlu, viewModel.CaleLogo);
        }

        public ListaPreparateUserControl(IMeniuRestaurantNavigator parent,
            ObservableCollection<Preparat> preparate, string titlu, string caleLogo) : this()
        {
            navigator = parent;
            viewModel = new ListaPreparateViewModel(parent, preparate, titlu, caleLogo);
            DataContext = viewModel;
            ConfigureazaVmEvents();
            ConfigureazaUI(titlu, caleLogo);
        }

        public ListaPreparateUserControl(AngajatHubView parent, Categorie categorie,
            TipCategorie tipParinte) : this()
        {
            parentViewBucatar = parent;
            viewModel = new ListaPreparateViewModel(null, categorie, tipParinte, ModListaPreparate.Edit);
            viewModel.InapoiEditRequested += () =>
                parent.NavigateBucatarLaListaCategorii(tipParinte);
            viewModel.DeschideEdit += item =>
            {
                if (item.EsteMeniu || item.Preparat == null) return;
                AdaugaPreparatView popup = new AdaugaPreparatView(item.Preparat.IdPreparat);
                popup.Owner = parent;
                popup.PreparatModificat += () => viewModel.IncarcaProduse();
                popup.ShowDialog();
            };
            DataContext = viewModel;
            ConfigureazaUI(categorie.Denumire, "/Images/carol_logo.png");
        }

        private void ConfigureazaVmEvents()
        {
            if (viewModel == null) return;
            viewModel.DeschideDetaliiBrowse += item =>
            {
                if (item.EsteMeniu && item.Meniu != null)
                {
                    var popup = new DetaliuMeniuView(item.Meniu) { Owner = navigator?.GetHostWindow() };
                    popup.ShowDialog();
                }
                else if (item.Preparat != null)
                {
                    var popup = new DetaliuPreparatView(item.Preparat) { Owner = navigator?.GetHostWindow() };
                    popup.ShowDialog();
                }
            };
        }

        private void ConfigureazaUI(string titlu, string caleLogo)
        {
            titluText.Text = titlu;
            try
            {
                logoImage.ImageSource = new BitmapImage(
                    new Uri($"pack://application:,,,{caleLogo}", UriKind.Absolute));
            }
            catch { }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel?.InapoiCommand.CanExecute(null) == true)
                viewModel.InapoiCommand.Execute(null);
        }

        private void Produs_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CatalogItem item)
                viewModel?.SelecteazaProdusCommand.Execute(item);
        }
    }
}
