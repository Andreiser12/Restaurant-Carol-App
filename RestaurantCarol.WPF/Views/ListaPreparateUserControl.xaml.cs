using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public enum ModListaPreparate
    {
        Browse,
        Edit
    }

    public partial class ListaPreparateUserControl : UserControl
    {
        private MeniuRestaurantView? parentViewClient;
        private AngajatHubView? parentViewBucatar;
        private PreparatBLL preparatBLL = new PreparatBLL();
        private TipCategorie? tipCategorieParinte;
        private Categorie? categorieCurenta;
        private ModListaPreparate mod;

        public ListaPreparateUserControl()
        {
            InitializeComponent();
        }

        public ListaPreparateUserControl(MeniuRestaurantView parent, Categorie categorie,
                                          TipCategorie tipParinte) : this()
        {
            parentViewClient = parent;
            categorieCurenta = categorie;
            tipCategorieParinte = tipParinte;
            mod = ModListaPreparate.Browse;

            ConfigureazaUI(categorie.Denumire, "/Images/carol_logo.png");
            IncarcaPreparate();
        }

        public ListaPreparateUserControl(MeniuRestaurantView parent,
                                          ObservableCollection<Preparat> preparate,
                                          string titlu,
                                          string caleLogoCentral) : this()
        {
            parentViewClient = parent;
            mod = ModListaPreparate.Browse;
            tipCategorieParinte = null;

            ConfigureazaUI(titlu, caleLogoCentral);
            DataContext = preparate;
        }

        public ListaPreparateUserControl(AngajatHubView parent, Categorie categorie,
                                          TipCategorie tipParinte) : this()
        {
            parentViewBucatar = parent;
            categorieCurenta = categorie;
            tipCategorieParinte = tipParinte;
            mod = ModListaPreparate.Edit;

            ConfigureazaUI(categorie.Denumire, "/Images/carol_logo.png");
            IncarcaPreparate();
        }

        private void ConfigureazaUI(string titlu, string caleLogo)
        {
            titluText.Text = titlu;

            try
            {
                BitmapImage bitmap = new BitmapImage(
                    new Uri($"pack://application:,,,{caleLogo}", UriKind.Absolute));
                logoImage.ImageSource = bitmap;
            }
            catch { }
        }

        public void IncarcaPreparate()
        {
            if (categorieCurenta == null) return;

            ObservableCollection<Preparat> preparate =
                preparatBLL.GetByCategorie(categorieCurenta.IdCategorie);
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
            catch { }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (mod == ModListaPreparate.Browse)
            {
                if (tipCategorieParinte.HasValue)
                {
                    string titlu = tipCategorieParinte.Value == TipCategorie.Mancare
                        ? "Mancare" : "Bauturi";
                    string caleImagine = tipCategorieParinte.Value == TipCategorie.Mancare
                        ? "/Images/categorie_mancare.jpg" : "/Images/categorie_bauturi.jpg";

                    parentViewClient?.NavigateToListaCategorii(tipCategorieParinte.Value,
                        titlu, caleImagine);
                }
                else
                {
                    parentViewClient?.NavigateToHub();
                }
            }
            else if (mod == ModListaPreparate.Edit)
            {
                parentViewBucatar?.NavigateBucatarLaListaCategorii(
                    tipCategorieParinte ?? TipCategorie.Mancare);
            }
        }

        private void Preparat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Preparat preparat)
            {
                if (mod == ModListaPreparate.Browse)
                {
                    DetaliuPreparatView popup = new DetaliuPreparatView(preparat);
                    popup.Owner = parentViewClient;
                    popup.ShowDialog();
                }
                else if (mod == ModListaPreparate.Edit)
                {
                    AdaugaPreparatView popup = new AdaugaPreparatView(preparat.IdPreparat);
                    if (parentViewBucatar != null)
                        popup.Owner = parentViewBucatar;

                    popup.PreparatModificat += OnPreparatModificat;
                    popup.ShowDialog();
                    popup.PreparatModificat -= OnPreparatModificat;
                }
            }
        }

        private void OnPreparatModificat()
        {
            IncarcaPreparate();
        }
    }
}