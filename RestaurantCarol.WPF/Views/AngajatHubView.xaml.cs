using System.Windows;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class AngajatHubView : Window
    {
        private AngajatHubViewModel viewModel;

        public AngajatHubView()
        {
            InitializeComponent();

            viewModel = new AngajatHubViewModel();
            DataContext = viewModel;

            viewModel.LogoutRequested += FaLogout;

            NavigateToHub();
        }

        public void NavigateToHub() =>
            contentArea.Content = new AngajatHubUserControl(this);

        public void NavigateToBucatar() =>
            contentArea.Content = new BucatarUserControl(this);

        public void NavigateToManager() =>
            contentArea.Content = new ManagerUserControl(this);

        public void NavigateToLivrator() =>
            contentArea.Content = new LivratorUserControl(this);

        public void NavigateLivratorLaComenzi() =>
            contentArea.Content = new LivratorComenziUserControl(this);

        public void NavigateBucatarLaListaCategorii(TipCategorie tip)
        {
            string titlu = tip == TipCategorie.Mancare ? "Mancare" : "Bauturi";
            string caleImagine = tip == TipCategorie.Mancare
                ? "/Images/categorie_mancare.jpg"
                : "/Images/categorie_bauturi.jpg";
            contentArea.Content = new ListaCategoriiUserControl(this, tip, titlu, caleImagine);
        }

        public void NavigateBucatarLaListaPreparate(Categorie categorie, TipCategorie tipParinte) =>
            contentArea.Content = new ListaPreparateUserControl(this, categorie, tipParinte);

        public void NavigateManagerLaComenzi(bool doarActive) =>
            contentArea.Content = new ManagerComenziUserControl(this, doarActive);

        public void NavigateManagerLaStocRedus() =>
            contentArea.Content = new ManagerStocRedusUserControl(this);

        private void FaLogout()
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}