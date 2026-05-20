using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
using RestaurantCarol.Views.Navigation;
using System.Windows;

namespace RestaurantCarol.Views
{
    public partial class MeniuRestaurantView : Window, IMeniuRestaurantNavigator
    {
        private MeniuRestaurantViewModel viewModel;

        public MeniuRestaurantView()
        {
            InitializeComponent();

            viewModel = new MeniuRestaurantViewModel();
            DataContext = viewModel;

            viewModel.CosRequested += DeschideCos;
            viewModel.AdresaRequested += DeschideAdrese;
            viewModel.StareComandaRequested += DeschideStareComanda;
            viewModel.LogoutRequested += FaLogout;
            viewModel.LoginRequested += DeschideLogin;
            viewModel.RegisterRequested += DeschideRegister;

            NavigateToHub();
        }

        public Window? GetHostWindow() => this;

        public void NavigateToHub()
        {
            contentArea.Content = new HubUserControl(this);
        }

        public void NavigateToListaCategorii(TipCategorie tip, string titlu, string caleImagine)
        {
            contentArea.Content = new ListaCategoriiUserControl(this, tip, titlu, caleImagine);
        }

        public void NavigateToListaPreparate(Categorie categorie, TipCategorie tipParinte)
        {
            contentArea.Content = new ListaPreparateUserControl(this, categorie, tipParinte);
        }

        public void NavigateToListaPreparatePopulare()
        {
            try
            {
                PreparatBLL bll = new PreparatBLL();
                var preparatePopulare = bll.GetTopPopulare(3);
                if (preparatePopulare.Count == 0)
                {
                    MessageBox.Show(
                        "Nu exista inca destule comenzi pentru a determina cele mai populare preparate.\n\n" +
                        "Plaseaza cateva comenzi si revino mai tarziu.",
                        "Cele mai populare",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                contentArea.Content = new ListaPreparateUserControl(
                    this,
                    preparatePopulare,
                    "Cele mai populare",
                    "/Images/categorie_populare.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare populare: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ActualizeazaStareComanda(ComandaBLL.RezultatPlasareComanda rezultat)
        {
            viewModel.ActualizeazaStareComanda(rezultat);
        }

        public void ActualizeazaPuncte()
        {
            viewModel.ActualizeazaPuncte();
        }

        private void DeschideCos()
        {
            CosView cos = new CosView(this);
            cos.Owner = this;
            cos.ShowDialog();
        }

        private void DeschideAdrese()
        {
            if (UserSession.CurrentUser == null) return;
            AdreseView adreseView = new AdreseView(UserSession.CurrentUser.IdUtilizator);
            adreseView.Owner = this;
            adreseView.AdreseModificate += viewModel.ActualizeazaAdresaImplicita;
            adreseView.ShowDialog();
            adreseView.AdreseModificate -= viewModel.ActualizeazaAdresaImplicita;
        }

        private void DeschideStareComanda()
        {
            StareComandaListView popup = new StareComandaListView();
            popup.Owner = this;
            popup.ComenziActualizate += viewModel.IncarcaStareComandaDinDb;
            popup.ShowDialog();
            popup.ComenziActualizate -= viewModel.IncarcaStareComandaDinDb;
        }

        private void FaLogout()
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void DeschideLogin()
        {
            LoginView login = new LoginView();
            login.Show();
            this.Close();
        }

        private void DeschideRegister()
        {
            RegisterView register = new RegisterView();
            register.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            viewModel.Detach();
        }
    }
}