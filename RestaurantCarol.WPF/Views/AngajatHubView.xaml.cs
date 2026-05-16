using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class AngajatHubView : Window
    {
        private Popup? prenumeMenuPopup;

        public AngajatHubView()
        {
            InitializeComponent();
            ConfigureazaUI();
            ConstruiestePopupPrenume();
            NavigateToHub();
        }

        public void NavigateToHub()
        {
            contentArea.Content = new AngajatHubUserControl(this);
        }

        public void NavigateToBucatar()
        {
            contentArea.Content = new BucatarUserControl(this);
        }

        public void NavigateToManager()
        {
            contentArea.Content = new ManagerUserControl(this);
        }

        public void NavigateToLivrator()
        {
            contentArea.Content = new LivratorUserControl(this);
        }

        public void NavigateLivratorLaComenzi()
        {
            contentArea.Content = new LivratorComenziUserControl(this);
        }

        private void ConfigureazaUI()
        {
            if (UserSession.CurrentUser == null) return;

            prenumeText.Text = UserSession.CurrentUser.Prenume;
            welcomeText.Text = $"Restaurant Carol - {UserSession.CurrentUser.Rol}";
        }

        private void ActiveazaButon(Button btn, ImageBrush img)
        {
            btn.IsEnabled = true;
            btn.Cursor = Cursors.Hand;
            img.RelativeTransform = null;
        }

        private void DezactiveazaButon(Button btn, ImageBrush img)
        {
            btn.IsEnabled = false;
            btn.Cursor = Cursors.No;
            img.Opacity = 0.3;
        }


        private void ConstruiestePopupPrenume()
        {
            prenumeMenuPopup = new Popup
            {
                PlacementTarget = prenumeBorder,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade
            };

            Border container = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(5, 8, 5, 8),
                MinWidth = 160
            };
            container.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 15,
                ShadowDepth = 0,
                Opacity = 0.3
            };

            StackPanel menu = new StackPanel();

            Button btnLogout = new Button
            {
                Content = "Iesire din cont",
                Background = Brushes.Transparent,
                Foreground = (Brush)new BrushConverter().ConvertFrom("#D85A4A")!,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(20, 12, 20, 12),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Cursor = Cursors.Hand
            };
            btnLogout.Click += Logout_Click;
            menu.Children.Add(btnLogout);

            container.Child = menu;
            prenumeMenuPopup.Child = container;
        }

        private void Prenume_Click(object sender, MouseButtonEventArgs e)
        {
            if (prenumeMenuPopup != null)
                prenumeMenuPopup.IsOpen = !prenumeMenuPopup.IsOpen;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Sigur vrei sa iesi din cont?",
                "Confirmare iesire",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            if (prenumeMenuPopup != null) prenumeMenuPopup.IsOpen = false;

            UserSession.Logout();

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        public void NavigateBucatarLaListaCategorii(TipCategorie tip)
        {
            string titlu = tip == TipCategorie.Mancare ? "Mancare" : "Bauturi";
            string caleImagine = tip == TipCategorie.Mancare
                ? "/Images/categorie_mancare.jpg"
                : "/Images/categorie_bauturi.jpg";

            contentArea.Content = new ListaCategoriiUserControl(this, tip, titlu, caleImagine);
        }

        public void NavigateBucatarLaListaPreparate(Categorie categorie, TipCategorie tipParinte)
        {
            contentArea.Content = new ListaPreparateUserControl(this, categorie, tipParinte);
        }

        public void NavigateManagerLaComenzi(bool doarActive)
        {
            contentArea.Content = new ManagerComenziUserControl(this, doarActive);
        }

        public void NavigateManagerLaStocRedus()
        {
            contentArea.Content = new ManagerStocRedusUserControl(this);
        }

        private void Bucatar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Livrator_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}