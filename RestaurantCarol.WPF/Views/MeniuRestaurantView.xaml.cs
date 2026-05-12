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

        private TextBlock? cosCountText;
        private TextBlock? cosTotalText;
        private Border? cosBorder;

        private void AfiseazaPanouClient()
        {
            cosBorder = new Border
            {
                Style = (Style)FindResource("InfoPanel"),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            cosBorder.MouseLeftButtonDown += Cos_Click;

            StackPanel sp1 = new StackPanel();
            sp1.Children.Add(new TextBlock
            {
                Text = "COS",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            cosCountText = new TextBlock
            {
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            sp1.Children.Add(cosCountText);

            cosTotalText = new TextBlock
            {
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = System.Windows.Media.Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };
            sp1.Children.Add(cosTotalText);

            cosBorder.Child = sp1;
            panouDreapta.Children.Add(cosBorder);

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

            CartSession.CartChanged += ActualizeazaPanouCos;
            ActualizeazaPanouCos();
        }

        private void ActualizeazaPanouCos()
        {
            if (cosCountText == null || cosTotalText == null) return;

            if (CartSession.EsteGol)
            {
                cosCountText.Text = "Cosul este gol";
                cosTotalText.Text = "";
            }
            else
            {
                int nr = CartSession.NumarTotalProduse;
                cosCountText.Text = $"{nr} {(nr == 1 ? "produs" : "produse")}";
                cosTotalText.Text = $"{CartSession.CostTotal:F2} RON";
            }
        }

        private void Cos_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CartSession.EsteGol)
            {
                MessageBox.Show("Cosul tau este gol. Adauga produse din meniu.",
                    "Cos gol", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CosView cos = new CosView();
            cos.Owner = this;
            cos.ShowDialog();
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CartSession.CartChanged -= ActualizeazaPanouCos;
        }
    }
}