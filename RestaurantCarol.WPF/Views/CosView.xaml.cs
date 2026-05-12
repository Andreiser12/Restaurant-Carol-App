using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class CosView : Window
    {
        public CosView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };

            itemsList.DataContext = CartSession.Items;

            CartSession.CartChanged += ActualizeazaUI;
            ActualizeazaUI();
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
            }
            else
            {
                itemsList.Visibility = Visibility.Visible;
                cosGolText.Visibility = Visibility.Collapsed;
                plaseazaButton.IsEnabled = true;
                plaseazaButton.Opacity = 1.0;
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
            MessageBox.Show(
                $"TODO: Plasare comanda\n\nTotal: {CartSession.CostTotal:F2} RON\nNr produse: {CartSession.NumarTotalProduse}",
                "Not implemented",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
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