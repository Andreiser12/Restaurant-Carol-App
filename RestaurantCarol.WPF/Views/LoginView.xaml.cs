using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
using System.Windows;
using System.Windows.Controls;
namespace RestaurantCarol.Views
{
    public partial class LoginView : Window
    {
        private LoginViewModel viewModel;
        public LoginView()
        {
            InitializeComponent();
            viewModel = new LoginViewModel
            {
                OnLoginSuccess = HandleLoginSuccess
            };
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb && viewModel != null)
            {
                viewModel.Parola = pb.Password;
            }
        }

        private void HandleLoginSuccess()
        {
            MessageBox.Show($"Bine ai venit, {UserSession.CurrentUser?.Prenume}!",
                "Login reusit", MessageBoxButton.OK, MessageBoxImage.Information);
            if (UserSession.IsClient)
            {
                MeniuRestaurantView meniu = new MeniuRestaurantView();
                meniu.Show();
            }
            else if (UserSession.IsAngajat)
            {
                AngajatHubView hub = new AngajatHubView();
                hub.Show();
            }
            this.Close();
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}