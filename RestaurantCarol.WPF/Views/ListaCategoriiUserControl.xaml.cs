using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class ListaCategoriiUserControl : UserControl
    {
        private MeniuRestaurantView? parentView;
        private CategorieBLL categorieBLL = new CategorieBLL();
        private TipCategorie tipParinte;

        public ListaCategoriiUserControl()
        {
            InitializeComponent();
        }

        public ListaCategoriiUserControl(MeniuRestaurantView parent, TipCategorie tip,
                                          string titlu, string caleImagine) : this()
        {
            parentView = parent;
            tipParinte = tip;
            titluText.Text = titlu;

            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri($"pack://application:,,,{caleImagine}", UriKind.Absolute));
                logoImage.ImageSource = bitmap;
            }
            catch
            {
                
            }

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
                parentView?.NavigateToListaPreparate(categorie, tipParinte);
            }
        }
    }
}