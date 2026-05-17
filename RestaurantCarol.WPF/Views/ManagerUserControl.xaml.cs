using System.Windows;
using System.Windows.Controls;
namespace RestaurantCarol.Views
{
    public partial class ManagerUserControl : UserControl
    {
        private AngajatHubView? parentView;
        public ManagerUserControl(AngajatHubView parent)
        {
            InitializeComponent();
            parentView = parent;
        }
        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToHub();
        }
        private void ToateComenzile_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateManagerLaComenzi(doarActive: false);
        }
        private void ComenziActive_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateManagerLaComenzi(doarActive: true);
        }
        private void StocRedus_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateManagerLaStocRedus();
        }
    }
}
