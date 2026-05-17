using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;
namespace RestaurantCarol.ViewModels
{
    public class RegisterViewModel : BasePropertyChanged
    {
        private RegisterBLL registerBLL = new RegisterBLL();
        public Action? OnRegisterSuccess { get; set; }
        private string nume = string.Empty;
        public string Nume
        {
            get => nume;
            set { nume = value; NotifyPropertyChanged(); }
        }
        private string prenume = string.Empty;
        public string Prenume
        {
            get => prenume;
            set { prenume = value; NotifyPropertyChanged(); }
        }
        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; NotifyPropertyChanged(); }
        }
        private string telefon = string.Empty;
        public string Telefon
        {
            get => telefon;
            set { telefon = value; NotifyPropertyChanged(); }
        }
        private string adresaLivrare = string.Empty;
        public string AdresaLivrare
        {
            get => adresaLivrare;
            set { adresaLivrare = value; NotifyPropertyChanged(); }
        }
        private string parola = string.Empty;
        public string Parola
        {
            get => parola;
            set { parola = value; NotifyPropertyChanged(); }
        }
        private string confirmaParola = string.Empty;
        public string ConfirmaParola
        {
            get => confirmaParola;
            set { confirmaParola = value; NotifyPropertyChanged(); }
        }
        private ICommand? registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                registerCommand ??= new RelayCommand<object>(_ => ExecuteRegister());
                return registerCommand;
            }
        }
        private void ExecuteRegister()
        {
            Utilizator utilizator = registerBLL.Register(
                Nume, Prenume, Email, Telefon, AdresaLivrare, Parola, ConfirmaParola);
            UserSession.Login(utilizator);
            OnRegisterSuccess?.Invoke();
        }
    }
}