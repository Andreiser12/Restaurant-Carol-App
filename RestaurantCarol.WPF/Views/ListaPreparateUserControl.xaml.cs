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

            SetLogo("/Images/carol_logo.png");

            ObservableCollection<Preparat> preparate = preparatBLL.GetByCategorie(categorie.IdCategorie);
            DataContext = preparate;
        }

        public ListaPreparateUserControl(MeniuRestaurantView parent,
                                          ObservableCollection<Preparat> preparate,
                                          string titlu,
                                          string caleLogoCentral) : this()
        {
            parentView = parent;
            tipCategorieParinte = null;

            titluText.Text = titlu;
            SetLogo(caleLogoCentral);

            DataContext = preparate;
        }

        private void SetLogo(string caleImagine)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(
                    new Uri($"pack://application:,,,{caleImagine}", UriKind.Absolute));
                logoImage.ImageSource = bitmap;
            }
            catch {}
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