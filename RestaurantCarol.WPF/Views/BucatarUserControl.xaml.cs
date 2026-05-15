using System.Windows;
using System.Windows.Controls;

namespace RestaurantCarol.Views
{
    public partial class BucatarUserControl : UserControl
    {
        private AngajatHubView? parentView;

        public BucatarUserControl()
        {
            InitializeComponent();
        }

        public BucatarUserControl(AngajatHubView parent) : this()
        {
            parentView = parent;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToHub();
        }

        private void AdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ModificaProdus_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Stoc_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}