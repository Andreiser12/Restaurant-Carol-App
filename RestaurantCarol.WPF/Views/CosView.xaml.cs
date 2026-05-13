using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class CosView : Window
    {
        private AdresaBLL adresaBLL = new AdresaBLL();

        public CosView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };

            itemsList.DataContext = CartSession.Items;

            IncarcaAdrese();

            CartSession.CartChanged += ActualizeazaUI;
            ActualizeazaUI();
        }

        private void IncarcaAdrese()
        {
            if (UserSession.CurrentUser == null) return;

            try
            {
                var adrese = adresaBLL.GetByUtilizator(UserSession.CurrentUser.IdUtilizator);

                if (adrese.Count == 0)
                {
                    adreseComboBox.Visibility = Visibility.Collapsed;
                    fariAdresaText.Visibility = Visibility.Visible;
                    plaseazaButton.IsEnabled = false;
                    plaseazaButton.Opacity = 0.5;
                }
                else
                {
                    adreseComboBox.Visibility = Visibility.Visible;
                    fariAdresaText.Visibility = Visibility.Collapsed;
                    adreseComboBox.ItemsSource = adrese;

                    var implicita = adrese.FirstOrDefault(a => a.EsteImplicita);
                    if (implicita != null)
                    {
                        adreseComboBox.SelectedItem = implicita;
                    }
                    else
                    {
                        adreseComboBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare adrese: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizeazaUI()
        {
            totalText.Text = $"{CartSession.CostTotal:F2} RON";

            if (CartSession.EsteGol)
            {
                itemsList.Visibility = Visibility.Collapsed;
                cosGolText.Visibility = Visibility.Visible;
                plaseazaButton.IsEnabled = false;
                plaseazaButton.Opacity = 0.5;
                adresaSection.Visibility = Visibility.Collapsed;
            }
            else
            {
                itemsList.Visibility = Visibility.Visible;
                cosGolText.Visibility = Visibility.Collapsed;
                adresaSection.Visibility = Visibility.Visible;

                bool areAdresaSelectata = adreseComboBox.SelectedItem != null;
                plaseazaButton.IsEnabled = areAdresaSelectata;
                plaseazaButton.Opacity = areAdresaSelectata ? 1.0 : 0.5;
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItem item)
            {
                CartSession.ModificaCantitate(item, item.Cantitate - 1);
            }
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItem item)
            {
                CartSession.ModificaCantitate(item, item.Cantitate + 1);
            }
        }

        private void Sterge_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CartItem item)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Sigur vrei sa stergi {item.Denumire} din cos?",
                    "Confirmare stergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    CartSession.Sterge(item);
                }
            }
        }

        private void Plaseaza_Click(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn || UserSession.CurrentUser == null)
            {
                MessageBox.Show("Trebuie sa fii logat pentru a plasa o comanda.",
                    "Nelogat", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (adreseComboBox.SelectedItem is not Adresa adresaSelectata)
            {
                MessageBox.Show("Selecteaza o adresa de livrare.",
                    "Adresa lipsa", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal totalEstimat = CartSession.CostTotal;
            MessageBoxResult confirm = MessageBox.Show(
                $"Sigur vrei sa plasezi comanda?\n\n" +
                $"Numar produse: {CartSession.NumarTotalProduse}\n" +
                $"Total mancare: {totalEstimat:F2} RON\n" +
                $"Adresa: {adresaSelectata.AdresaText}\n\n" +
                $"(Transportul si discount-urile se calculeaza la plasare)",
                "Confirmare comanda",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                ComandaBLL comandaBLL = new ComandaBLL();
                var rezultat = comandaBLL.PlaseazaComanda(
                    UserSession.CurrentUser.IdUtilizator,
                    adresaSelectata.IdAdresa,
                    CartSession.Items);

                string mesaj = $"Comanda plasata cu succes!\n\n" +
                               $"Cod comanda: {rezultat.CodComanda}\n" +
                               $"Cost mancare: {rezultat.CostMancare:F2} RON\n";

                if (rezultat.Discount > 0)
                    mesaj += $"Discount: -{rezultat.Discount:F2} RON\n";

                mesaj += $"Cost transport: {rezultat.CostTransport:F2} RON\n";

                if (rezultat.CostTransport == 0)
                    mesaj += "(Transport gratuit)\n";

                mesaj += $"\nTotal: {rezultat.CostTotal:F2} RON\n" +
                         $"Adresa: {adresaSelectata.AdresaText}\n" +
                         $"Ora estimata livrare: {rezultat.OraEstimataLivrare:HH:mm}";

                MessageBox.Show(mesaj, "Comanda plasata",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (Owner is MeniuRestaurantView meniuView)
                {
                    meniuView.ActualizeazaStareComanda(rezultat);
                }

                CartSession.Goleste();

                this.Close();
            }
            catch (RestaurantException ex)
            {
                MessageBox.Show(ex.Message, "Eroare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A aparut o eroare neasteptata: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CartSession.CartChanged -= ActualizeazaUI;
        }
    }
}

