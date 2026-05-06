using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class ListaCategoriiUserControl : UserControl
    {
        private MeniuRestaurantView? parentView;
        private CategorieBLL categorieBLL = new CategorieBLL();

        public ListaCategoriiUserControl()
        {
            InitializeComponent();
        }

        public ListaCategoriiUserControl(MeniuRestaurantView parent, TipCategorie tip, string titlu) : this()
        {
            parentView = parent;
            titluText.Text = titlu;

            ObservableCollection<Categorie> categorii = categorieBLL.GetCategoriiByTip(tip);
            DataContext = categorii;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToHub();
        }

        private void Categorie_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Categorie categorie)
            {
            }
        }
    }
}