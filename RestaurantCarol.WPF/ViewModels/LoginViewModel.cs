using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class LoginViewModel : BasePropertyChanged
    {
        private AutentificareBLL autentificareBLL = new AutentificareBLL();

        public Action? OnLoginSuccess { get; set; }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; NotifyPropertyChanged(); }
        }

        private string parola = string.Empty;
        public string Parola
        {
            get => parola;
            set { parola = value; NotifyPropertyChanged(); }
        }

        private ICommand? loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                loginCommand ??= new RelayCommand<object>(_ => ExecuteLogin());
                return loginCommand;
            }
        }

        private void ExecuteLogin()
        {
            Utilizator utilizator = autentificareBLL.Autentificare(Email, Parola);
            UserSession.Login(utilizator);
            OnLoginSuccess?.Invoke();
        }
    }
}