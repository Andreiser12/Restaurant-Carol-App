using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;
namespace RestaurantCarol.Views
{
    public partial class LivratorUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private ComandaBLL comandaBLL = new ComandaBLL();
        public LivratorUserControl()
        {
            InitializeComponent();
        }
        public LivratorUserControl(AngajatHubView parent) : this()
        {
            parentView = parent;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LivratorSession.Reset();
            parentView?.NavigateToHub();
        }
        private void VeziComenzi_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateLivratorLaComenzi();
        }
        private void AmAjuns_Click(object sender, RoutedEventArgs e)
        {
            if (LivratorSession.ComandaSelectata == null)
            {
                MessageBox.Show(
                    "Selecteaza mai intai o comanda din „Vezi comenzi”.",
                    "Nicio comanda selectata",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            var comanda = LivratorSession.ComandaSelectata;
            try
            {
                comandaBLL.ConfirmaLivrare(comanda.IdComanda);
                MessageBox.Show(
                    $"Comanda #{comanda.CodComanda} a fost marcata ca livrata.",
                    "Succes",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                LivratorSession.Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
