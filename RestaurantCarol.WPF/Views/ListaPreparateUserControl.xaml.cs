using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class ListaPreparateUserControl : UserControl
    {
        private MeniuRestaurantView? parentView;
        private PreparatBLL preparatBLL = new PreparatBLL();

        private TipCategorie? tipCategorieParinte;

        public ListaPreparateUserControl()
        {
            InitializeComponent();
        }

        public ListaPreparateUserControl(MeniuRestaurantView parent, Categorie categorie,
                                          TipCategorie tipParinte) : this()
        {
            parentView = parent;
            tipCategorieParinte = tipParinte;

            titluText.Text = categorie.Denumire;

            try
            {
                BitmapImage bitmap = new BitmapImage(
                    new Uri("pack://application:,,,/Images/carol_logo.png", UriKind.Absolute));
                logoImage.ImageSource = bitmap;
            }
            catch {}

            ObservableCollection<Preparat> preparate = preparatBLL.GetByCategorie(categorie.IdCategorie);
            DataContext = preparate;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (tipCategorieParinte.HasValue)
            {
                string titlu = tipCategorieParinte.Value == TipCategorie.Mancare
                    ? "Mancare" : "Bauturi";
                string caleImagine = tipCategorieParinte.Value == TipCategorie.Mancare
                    ? "/Images/categorie_mancare.jpg" : "/Images/categorie_bauturi.jpg";

                parentView?.NavigateToListaCategorii(tipCategorieParinte.Value, titlu, caleImagine);
            }
            else
            {
                parentView?.NavigateToHub();
            }
        }

        private void Preparat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Preparat preparat)
            {
                DetaliuPreparatView popup = new DetaliuPreparatView(preparat);
                popup.Owner = parentView;
                popup.ShowDialog();
            }
        }
    }
}