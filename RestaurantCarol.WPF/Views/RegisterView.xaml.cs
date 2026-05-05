using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class RegisterView : Window
    {
        private RegisterViewModel viewModel;

        public RegisterView()
        {
            InitializeComponent();

            viewModel = new RegisterViewModel
            {
                OnRegisterSuccess = HandleRegisterSuccess
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

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb && viewModel != null)
            {
                viewModel.ConfirmaParola = pb.Password;
            }
        }

        private void HandleRegisterSuccess()
        {
            MessageBox.Show(
                $"Bine ai venit, {Layers.UserSession.CurrentUser?.Prenume}!\n\nContul tau a fost creat cu succes.",
                "Inregistrare reusita", MessageBoxButton.OK, MessageBoxImage.Information);

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