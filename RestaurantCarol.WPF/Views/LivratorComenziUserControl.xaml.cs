using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class LivratorComenziUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private ComandaBLL comandaBLL = new ComandaBLL();

        public LivratorComenziUserControl(AngajatHubView parent)
        {
            InitializeComponent();
            parentView = parent;
            IncarcaComenzi();
        }

        private void IncarcaComenzi()
        {
            try
            {
                var comenzi = comandaBLL.GetComenziLivrator();
                comenziList.ItemsSource = comenzi;
                emptyText.Visibility = comenzi.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                if (LivratorSession.ComandaSelectata != null)
                {
                    foreach (Comanda comanda in comenziList.Items)
                    {
                        if (comanda.IdComanda == LivratorSession.ComandaSelectata.IdComanda)
                        {
                            comenziList.SelectedItem = comanda;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea comenzilor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComenziList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LivratorSession.ComandaSelectata = comenziList.SelectedItem as Comanda;
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToLivrator();
        }
    }
}
