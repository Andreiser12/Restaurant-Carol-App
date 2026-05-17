using Microsoft.Win32;
using RestaurantCarol.Views;
using System.Diagnostics;
using System.Windows;
using RestaurantCarol.Layers;
namespace RestaurantCarol
{
    public partial class MainWindow : Window
    {
        private const string GOOGLE_FORM_URL = "https://forms.google.com/PLACEHOLDER";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AcceseazaCaOaspete_Click(object sender, RoutedEventArgs e)
        {
            MeniuRestaurantView meniu = new MeniuRestaurantView();
            meniu.Show();
            this.Close();
        }
        private void IntraInCont_Click(object sender, RoutedEventArgs e)
        {
            LoginView login = new LoginView();
            login.Show();
            this.Close();
        }
        private void Inregistreaza_Click(object sender, RoutedEventArgs e)
        {
            RegisterView register = new RegisterView();
            register.Show();
            this.Close();
        }
        private void Contacteaza_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = GOOGLE_FORM_URL,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nu s-a putut deschide formularul: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}