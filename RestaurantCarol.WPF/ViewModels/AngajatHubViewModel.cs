using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AngajatHubViewModel : ViewModelBase
    {
        public event Action? LogoutRequested;

        private string prenumeText = string.Empty;
        public string PrenumeText
        {
            get => prenumeText;
            set { prenumeText = value; NotifyPropertyChanged(); }
        }

        private string welcomeText = string.Empty;
        public string WelcomeText
        {
            get => welcomeText;
            set { welcomeText = value; NotifyPropertyChanged(); }
        }

        private bool popupPrenumeDeschis;
        public bool PopupPrenumeDeschis
        {
            get => popupPrenumeDeschis;
            set { popupPrenumeDeschis = value; NotifyPropertyChanged(); }
        }

        public AngajatHubViewModel()
        {
            ConfigureazaUI();
        }

        private void ConfigureazaUI()
        {
            if (UserSession.CurrentUser == null) return;

            PrenumeText = UserSession.CurrentUser.Prenume;
            WelcomeText = $"Restaurant Carol - {UserSession.CurrentUser.Rol}";
        }

        public ICommand PrenumeCommand => new RelayCommand<object>(_ =>
            PopupPrenumeDeschis = !PopupPrenumeDeschis);

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
            LogoutRequested?.Invoke();
        });
    }
}