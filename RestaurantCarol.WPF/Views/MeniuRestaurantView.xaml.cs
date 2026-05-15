using RestaurantCarol.Layers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RestaurantCarol.Views
{
    public partial class MeniuRestaurantView : Window
    {
        private TextBlock? cosCountText;
        private TextBlock? cosTotalText;
        private Border? cosBorder;
        private TextBlock? adresaText;
        private Border? adresaBorder;
        private TextBlock? stareCmdCodText;
        private TextBlock? stareCmdStareText;
        private TextBlock? stareCmdOraText;
        private Border? prenumeBorder;
        private Popup? prenumeMenuPopup;

        private ComandaBLL.RezultatPlasareComanda? ultimaComandaPlasata;

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
                puncteBorder.Visibility = Visibility.Visible;
                ActualizeazaPuncte();
                AfiseazaPanouClient();
            }
            else
            {
                puncteBorder.Visibility = Visibility.Collapsed;
                AfiseazaPanouOaspete();
            }
        }

        private void AfiseazaPanouClient()
        {
            cosBorder = new Border
            {
                Style = (Style)FindResource("HeaderPanel"),
                Cursor = Cursors.Hand
            };

            cosBorder.MouseLeftButtonDown += Cos_Click;

            StackPanel sp1 = new StackPanel();
            sp1.Children.Add(new TextBlock
            {
                Text = "COS",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            cosCountText = new TextBlock
            {
                FontSize = 11,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            sp1.Children.Add(cosCountText);

            cosTotalText = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };
            sp1.Children.Add(cosTotalText);

            cosBorder.Child = sp1;
            panouDreapta.Children.Add(cosBorder);

            adresaBorder = new Border
            {
                Style = (Style)FindResource("HeaderPanel"),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            adresaBorder.MouseLeftButtonDown += Adresa_Click;

            StackPanel sp2 = new StackPanel();
            sp2.Children.Add(new TextBlock
            {
                Text = "ADRESA",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            adresaText = new TextBlock
            {
                FontSize = 11,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0),
                MaxWidth = 150
            };
            sp2.Children.Add(adresaText);

            adresaBorder.Child = sp2;
            panouDreapta.Children.Add(adresaBorder);

            ActualizeazaAdresaImplicita();

            Border borderStare = new Border
            {
                Style = (Style)FindResource("HeaderPanel")
            };
            StackPanel sp3 = new StackPanel();
            sp3.Children.Add(new TextBlock
            {
                Text = "STARE COMANDA",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            stareCmdCodText = new TextBlock
            {
                Text = "Nicio comanda activa",
                FontSize = 10,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            sp3.Children.Add(stareCmdCodText);

            stareCmdStareText = new TextBlock
            {
                Text = "",
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0),
            };
            sp3.Children.Add(stareCmdStareText);

            stareCmdOraText = new TextBlock
            {
                Text = "",
                FontSize = 10,
                FontStyle = FontStyles.Italic,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };
            sp3.Children.Add(stareCmdOraText);

            borderStare.Child = sp3;
            panouDreapta.Children.Add(borderStare);

            prenumeBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFrom("#EB7A6B14")!,
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(20, 12, 20, 12),
                Margin = new Thickness(5, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            prenumeBorder.MouseLeftButtonDown += Prenume_Click;

            TextBlock prenumeTxt = new TextBlock
            {
                Text = UserSession.CurrentUser?.Prenume ?? "Client",
                FontSize = 18,
                Foreground = Brushes.White
            };
            prenumeBorder.Child = prenumeTxt;
            panouDreapta.Children.Add(prenumeBorder);

            ConstruiestePopupPrenume();
            CartSession.CartChanged += ActualizeazaPanouCos;
            ActualizeazaPanouCos();
            ActualizeazaAdresaImplicita();
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

        public void ActualizeazaStareComanda(ComandaBLL.RezultatPlasareComanda rezultat)
        {
            ultimaComandaPlasata = rezultat;

            if (stareCmdCodText != null)
                stareCmdCodText.Text = $"Cod: {rezultat.CodComanda}";

            if (stareCmdStareText != null)
                stareCmdStareText.Text = "Inregistrata";

            if (stareCmdOraText != null)
                stareCmdOraText.Text = $"Livrare ~ {rezultat.OraEstimataLivrare:HH:mm}";

            ActualizeazaPuncte();
        }

        public void ActualizeazaPuncte()
        {
            if (puncteText != null && UserSession.CurrentUser != null)
            {
                puncteText.Text = $"Puncte Carol: {UserSession.CurrentUser.Puncte}";
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
            Button btnLogin = CreeazaButonHeader("Logheaza-te");
            btnLogin.Click += Login_Click;
            panouDreapta.Children.Add(btnLogin);

            Button btnRegister = CreeazaButonHeader("Inregistreaza-te");
            btnRegister.Click += Register_Click;
            panouDreapta.Children.Add(btnRegister);

            Border prenume = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFrom("#EB7A6B14")!,
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(20, 12, 20, 12),
                Margin = new Thickness(5, 0, 0, 0)
            };
            prenume.Child = new TextBlock
            {
                Text = "Oaspete",
                FontSize = 18,
                Foreground = Brushes.White
            };
            panouDreapta.Children.Add(prenume);
        }

        private Button CreeazaButonHeader(string text)
        {
            Button btn = new Button
            {
                Content = text,
                Background = (Brush)new BrushConverter().ConvertFrom("#F4B860")!,
                Foreground = (Brush)new BrushConverter().ConvertFrom("#3A3A3A")!,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Width = 140,
                Height = 45,
                Margin = new Thickness(5, 0, 5, 0),
                Cursor = Cursors.Hand,
                BorderThickness = new Thickness(0)
            };

            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            FrameworkElementFactory cp = new FrameworkElementFactory(typeof(ContentPresenter));
            cp.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cp.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(cp);
            template.VisualTree = border;
            btn.Template = template;

            return btn;
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
                Padding = new Thickness(0),
                Margin = new Thickness(5, 8, 5, 8),
                MinWidth = 160
            };
            container.Effect = new System.Windows.Media.Effects.DropShadowEffect
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
            CartSession.Goleste();

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
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
            LoginView login = new LoginView();
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

        private void ActualizeazaAdresaImplicita()
        {
            if (adresaText == null) return;

            if (UserSession.CurrentUser == null)
            {
                adresaText.Text = "Nicio adresa setata";
                return;
            }

            try
            {
                AdresaBLL adresaBLL = new AdresaBLL();
                Adresa? adresaImplicita = adresaBLL.GetAdresaImplicita(UserSession.CurrentUser.IdUtilizator);

                if (adresaImplicita != null)
                {
                    adresaText.Text = adresaImplicita.AdresaText;
                }
                else
                {
                    adresaText.Text = "Nicio adresa setata";
                }
            }
            catch
            {
                adresaText.Text = "Nicio adresa setata";
            }
        }

        private void Adresa_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UserSession.CurrentUser == null) return;

            AdreseView adreseView = new AdreseView(UserSession.CurrentUser.IdUtilizator);
            adreseView.Owner = this;

            adreseView.AdreseModificate += ActualizeazaAdresaImplicita;

            adreseView.ShowDialog();

            adreseView.AdreseModificate -= ActualizeazaAdresaImplicita;
        }
    }
}