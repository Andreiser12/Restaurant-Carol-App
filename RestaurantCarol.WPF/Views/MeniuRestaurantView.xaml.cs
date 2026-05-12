using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class MeniuRestaurantView : Window
    {
        public MeniuRestaurantView()
        {
            InitializeComponent();
            ConfigureazaUI();
            NavigateToHub();
        }

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

        private void ConfigureazaUI()
        {
            if (UserSession.IsLoggedIn && UserSession.IsClient)
            {
                prenumeText.Text = UserSession.CurrentUser?.Prenume ?? "Client";
                AfiseazaPanouClient();
            }
            else
            {
                AfiseazaPanouOaspete();
            }
        }

        private void AfiseazaPanouClient()
        {
            Border borderCos = new Border
            {
                Style = (Style)FindResource("InfoPanel")
            };
            StackPanel sp1 = new StackPanel();
            sp1.Children.Add(new TextBlock
            {
                Text = "COS",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            sp1.Children.Add(new TextBlock
            {
                Text = "Cosul este gol",
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            });
            borderCos.Child = sp1;
            panouDreapta.Children.Add(borderCos);

            Border borderAdresa = new Border
            {
                Style = (Style)FindResource("InfoPanel")
            };
            StackPanel sp2 = new StackPanel();
            sp2.Children.Add(new TextBlock
            {
                Text = "ADRESA",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            sp2.Children.Add(new TextBlock
            {
                Text = UserSession.CurrentUser?.AdresaLivrare ?? "Nicio adresa setata",
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            });
            borderAdresa.Child = sp2;
            panouDreapta.Children.Add(borderAdresa);

            Border borderStare = new Border
            {
                Style = (Style)FindResource("InfoPanel")
            };
            StackPanel sp3 = new StackPanel();
            sp3.Children.Add(new TextBlock
            {
                Text = "STARE COMANDA",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            sp3.Children.Add(new TextBlock
            {
                Text = "Nicio comanda activa",
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            });
            borderStare.Child = sp3;
            panouDreapta.Children.Add(borderStare);
        }

        private void AfiseazaPanouOaspete()
        {
            Button btnLogin = new Button
            {
                Content = "Logheaza-te",
                Style = (Style)FindResource("SidePanelButton")
            };
            btnLogin.Click += Login_Click;
            panouDreapta.Children.Add(btnLogin);

            Button btnRegister = new Button
            {
                Content = "Inregistreaza-te",
                Style = (Style)FindResource("SidePanelButton")
            };
            btnRegister.Click += Register_Click;
            panouDreapta.Children.Add(btnRegister);
        }

        private void Populare_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Mancare_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Bauturi_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginView login = new LoginView(RolUtilizator.Client);
            login.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterView register = new RegisterView();
            register.Show();
            this.Close();
        }
    }
}