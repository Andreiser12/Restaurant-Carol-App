using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class MeniuRestaurantViewModel : ViewModelBase
    {
        public event Action? CosRequested;
        public event Action? AdresaRequested;
        public event Action? StareComandaRequested;
        public event Action? LogoutRequested;
        public event Action? LoginRequested;
        public event Action? RegisterRequested;

        private AdresaBLL adresaBLL = new AdresaBLL();
        private ComandaBLL comandaBLL = new ComandaBLL();

        public bool EsteClient => UserSession.IsLoggedIn && UserSession.IsClient;
        public bool EsteOaspete => !EsteClient;

        public Visibility VizibilitatePanouClient =>
            EsteClient ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VizibilitatePanouOaspete =>
            EsteOaspete ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VizibilitatePuncte =>
            EsteClient ? Visibility.Visible : Visibility.Collapsed;

        private string puncteText = "Puncte Carol: 0";
        public string PuncteText
        {
            get => puncteText;
            set { puncteText = value; NotifyPropertyChanged(); }
        }

        private string cosCountText = "Cosul este gol";
        public string CosCountText
        {
            get => cosCountText;
            set { cosCountText = value; NotifyPropertyChanged(); }
        }

        private string cosTotalText = string.Empty;
        public string CosTotalText
        {
            get => cosTotalText;
            set { cosTotalText = value; NotifyPropertyChanged(); }
        }

        private string adresaText = "Nicio adresa setata";
        public string AdresaText
        {
            get => adresaText;
            set { adresaText = value; NotifyPropertyChanged(); }
        }

        private string stareCmdCodText = "Nicio comanda activa";
        public string StareCmdCodText
        {
            get => stareCmdCodText;
            set { stareCmdCodText = value; NotifyPropertyChanged(); }
        }

        private string stareCmdStareText = string.Empty;
        public string StareCmdStareText
        {
            get => stareCmdStareText;
            set { stareCmdStareText = value; NotifyPropertyChanged(); }
        }

        private string stareCmdOraText = string.Empty;
        public string StareCmdOraText
        {
            get => stareCmdOraText;
            set { stareCmdOraText = value; NotifyPropertyChanged(); }
        }

        private string prenumeText = "Client";
        public string PrenumeText
        {
            get => prenumeText;
            set { prenumeText = value; NotifyPropertyChanged(); }
        }

        private bool popupPrenumeDeschis;
        public bool PopupPrenumeDeschis
        {
            get => popupPrenumeDeschis;
            set { popupPrenumeDeschis = value; NotifyPropertyChanged(); }
        }

        public MeniuRestaurantViewModel()
        {
            ConfigureazaUI();
            CartSession.CartChanged += OnCartChanged;
        }

        private void ConfigureazaUI()
        {
            if (EsteClient)
            {
                PrenumeText = UserSession.CurrentUser?.Prenume ?? "Client";
                ActualizeazaPuncte();
                ActualizeazaPanouCos();
                ActualizeazaAdresaImplicita();
                IncarcaStareComandaDinDb();
            }
            else
            {
                PrenumeText = "Oaspete";
            }
        }

        public void ActualizeazaPuncte()
        {
            if (UserSession.CurrentUser != null)
                PuncteText = $"Puncte Carol: {UserSession.CurrentUser.Puncte}";
        }

        public void ActualizeazaPanouCos()
        {
            if (CartSession.EsteGol)
            {
                CosCountText = "Cosul este gol";
                CosTotalText = string.Empty;
            }
            else
            {
                int nr = CartSession.NumarTotalProduse;
                CosCountText = $"{nr} {(nr == 1 ? "produs" : "produse")}";
                CosTotalText = $"{CartSession.CostTotal:F2} RON";
            }
        }

        public void ActualizeazaAdresaImplicita()
        {
            if (UserSession.CurrentUser == null)
            {
                AdresaText = "Nicio adresa setata";
                return;
            }

            try
            {
                Adresa? adresaImplicita = adresaBLL.GetAdresaImplicita(UserSession.CurrentUser.IdUtilizator);
                AdresaText = adresaImplicita?.AdresaText ?? "Nicio adresa setata";
            }
            catch
            {
                AdresaText = "Nicio adresa setata";
            }
        }

        public void IncarcaStareComandaDinDb()
        {
            if (!UserSession.IsClient || UserSession.CurrentUser == null)
                return;

            try
            {
                Comanda? comanda = comandaBLL.GetUltimaComandaActivaClient(
                    UserSession.CurrentUser.IdUtilizator);

                if (comanda == null)
                {
                    StareCmdCodText = "Nicio comanda activa";
                    StareCmdStareText = string.Empty;
                    StareCmdOraText = string.Empty;
                    return;
                }

                StareCmdCodText = $"Cod: {comanda.CodComanda}";
                StareCmdStareText = comanda.StareAfisata;
                StareCmdOraText = comanda.OraEstimataLivrare.HasValue
                    ? $"Livrare ~ {comanda.OraEstimataLivrare:HH:mm}"
                    : string.Empty;
            }
            catch {}
        }

        public void ActualizeazaStareComanda(ComandaBLL.RezultatPlasareComanda rezultat)
        {
            StareCmdCodText = $"Cod: {rezultat.CodComanda}";
            StareCmdStareText = StareComandaHelper.GetDenumireAfisata(StareComanda.Inregistrata);
            StareCmdOraText = $"Livrare ~ {rezultat.OraEstimataLivrare:HH:mm}";
            ActualizeazaPuncte();
            IncarcaStareComandaDinDb();
        }

        private void OnCartChanged()
        {
            ActualizeazaPanouCos();
        }

        public void Detach()
        {
            CartSession.CartChanged -= OnCartChanged;
        }

        public ICommand CosCommand => new RelayCommand<object>(_ =>
        {
            if (CartSession.EsteGol)
            {
                MessageBox.Show("Cosul tau este gol. Adauga produse din meniu.",
                    "Cos gol", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CosRequested?.Invoke();
        });

        public ICommand AdresaCommand => new RelayCommand<object>(_ =>
        {
            if (UserSession.CurrentUser == null) return;
            AdresaRequested?.Invoke();
        });

        public ICommand StareComandaCommand => new RelayCommand<object>(_ =>
        {
            if (!UserSession.IsClient) return;
            StareComandaRequested?.Invoke();
        });

        public ICommand PrenumeCommand => new RelayCommand<object>(_ =>
        {
            PopupPrenumeDeschis = !PopupPrenumeDeschis;
        });

        public ICommand LogoutCommand => new RelayCommand<object>(_ =>
        {
            MessageBoxResult result = MessageBox.Show(
                "Sigur vrei sa iesi din cont?",
                "Confirmare iesire",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            PopupPrenumeDeschis = false;
            UserSession.Logout();
            CartSession.Goleste();
            LogoutRequested?.Invoke();
        });

        public ICommand LoginCommand => new RelayCommand<object>(_ =>
            LoginRequested?.Invoke());

        public ICommand RegisterCommand => new RelayCommand<object>(_ =>
            RegisterRequested?.Invoke());
    }
}