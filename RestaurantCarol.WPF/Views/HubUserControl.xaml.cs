using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class HubUserControl : UserControl
    {
        private MeniuRestaurantView? parentView;

        public HubUserControl()
        {
            InitializeComponent();
        }

        public HubUserControl(MeniuRestaurantView parent) : this()
        {
            parentView = parent;
        }

        private void Populare_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Mancare_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToListaCategorii(
                TipCategorie.Mancare,
                "Mancare",
                "/Images/categorie_mancare.png");
        }

        private void Bauturi_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}