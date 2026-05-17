using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;
namespace RestaurantCarol.Views
{
    public enum ModListaCategorii
    {
        Browse,
        Edit
    }
    public partial class ListaCategoriiUserControl : UserControl
    {
        private MeniuRestaurantView? parentViewClient;
        private AngajatHubView? parentViewBucatar;
        private CategorieBLL categorieBLL = new CategorieBLL();
        private TipCategorie tipParinte;
        private ModListaCategorii mod;
        public ListaCategoriiUserControl()
        {
            InitializeComponent();
        }
        public ListaCategoriiUserControl(MeniuRestaurantView parent, TipCategorie tip,
                                          string titlu, string caleImagine) : this()
        {
            parentViewClient = parent;
            tipParinte = tip;
            mod = ModListaCategorii.Browse;
            ConfigureazaUI(tip, titlu, caleImagine);
        }
        public ListaCategoriiUserControl(AngajatHubView parent, TipCategorie tip,
                                          string titlu, string caleImagine) : this()
        {
            parentViewBucatar = parent;
            tipParinte = tip;
            mod = ModListaCategorii.Edit;
            ConfigureazaUI(tip, titlu, caleImagine);
        }
        private void ConfigureazaUI(TipCategorie tip, string titlu, string caleImagine)
        {
            titluText.Text = titlu;
            try
            {
                BitmapImage bitmap = new BitmapImage(
                    new Uri($"pack://application:,,,{caleImagine}", UriKind.Absolute));
                logoImage.ImageSource = bitmap;
            }
            catch { }
            try
            {
                ObservableCollection<Categorie> categorii = categorieBLL.GetCategoriiByTip(tip);
                DataContext = categorii;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea categoriilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (mod == ModListaCategorii.Browse)
                parentViewClient?.NavigateToHub();
            else if (mod == ModListaCategorii.Edit)
                parentViewBucatar?.NavigateToBucatar();
        }
        private void Categorie_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Categorie categorie)
            {
                if (mod == ModListaCategorii.Browse)
                    parentViewClient?.NavigateToListaPreparate(categorie, tipParinte);
                else if (mod == ModListaCategorii.Edit)
                    parentViewBucatar?.NavigateBucatarLaListaPreparate(categorie, tipParinte);
            }
        }
    }
}