using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class DetaliuPreparatView : Window
    {
        private Preparat? preparat;
        private PreparatBLL preparatBLL = new PreparatBLL();
        private int cantitate = 1;

        public DetaliuPreparatView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }

        public DetaliuPreparatView(Preparat preparat) : this()
        {
            this.preparat = preparat;
            IncarcaDatePreparat();
        }

        private void IncarcaDatePreparat()
        {
            if (preparat == null) return;

            try
            {
                string calePoza = !string.IsNullOrEmpty(preparat.PrimaCalePoza)
                    ? preparat.PrimaCalePoza
                    : "/Images/carol_logo.png";

                string pathFinal = Helpers.ImageUploadHelper.ConstruiestePathPentruImage(calePoza);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(pathFinal, UriKind.Absolute);
                bitmap.EndInit();

                pozaPreparat.ImageSource = bitmap;
            }
            catch {}

            denumireText.Text = preparat.Denumire;
            pretText.Text = $"{preparat.Pret:F2} RON";
            cantitatePortieText.Text = $"{preparat.CantitatePortie}g";

            if (!string.IsNullOrWhiteSpace(preparat.Descriere))
            {
                descriereText.Text = preparat.Descriere;
            }
            else
            {
                ingredienteSection.Visibility = Visibility.Collapsed;
            }

            ObservableCollection<Alergen> alergeni = preparatBLL.GetAlergeniByPreparat(preparat.IdPreparat);
            if (alergeni.Count > 0)
            {
                alergeniList.ItemsSource = alergeni;
            }
            else
            {
                alergeniSection.Visibility = Visibility.Collapsed;
            }

            IncarcaValoriNutritionale();

            ConfigureazaSectiuneCos();
        }

        private void IncarcaValoriNutritionale()
        {
            if (preparat == null) return;

            bool areValori = false;

            if (preparat.Calorii.HasValue)
            {
                AdaugaRandValoare("Calorii", $"{preparat.Calorii.Value} kcal");
                areValori = true;
            }
            if (preparat.Grasimi.HasValue)
            {
                AdaugaRandValoare("Grăsimi", $"{preparat.Grasimi.Value:F2} g");
                areValori = true;
            }
            if (preparat.Carbohidrati.HasValue)
            {
                AdaugaRandValoare("Carbohidrați", $"{preparat.Carbohidrati.Value:F2} g");
                areValori = true;
            }
            if (preparat.Proteine.HasValue)
            {
                AdaugaRandValoare("Proteine", $"{preparat.Proteine.Value:F2} g");
                areValori = true;
            }
            if (preparat.Sare.HasValue)
            {
                AdaugaRandValoare("Sare", $"{preparat.Sare.Value:F2} g");
                areValori = true;
            }

            if (!areValori)
            {
                valoriSection.Visibility = Visibility.Collapsed;
            }
        }

        private void AdaugaRandValoare(string nume, string valoare)
        {
            Grid rand = new Grid();
            rand.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            rand.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            rand.Margin = new Thickness(0, 3, 0, 3);

            TextBlock numeText = new TextBlock
            {
                Text = nume,
                FontSize = 14,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush
            };
            Grid.SetColumn(numeText, 0);

            TextBlock valoareText = new TextBlock
            {
                Text = valoare,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = new BrushConverter().ConvertFromString("#D2AF6D") as Brush
            };
            Grid.SetColumn(valoareText, 1);

            rand.Children.Add(numeText);
            rand.Children.Add(valoareText);
            valoriList.Children.Add(rand);
        }

        private void ConfigureazaSectiuneCos()
        {
            if (UserSession.IsLoggedIn && UserSession.IsClient)
            {
                cosSection.Visibility = Visibility.Visible;
                oaspeteSection.Visibility = Visibility.Collapsed;
                ActualizeazaButonAdauga();
            }
            else
            {
                cosSection.Children[0].Visibility = Visibility.Collapsed;
                cosSection.Children[1].Visibility = Visibility.Collapsed;
                oaspeteSection.Visibility = Visibility.Visible;
            }
        }

        private void ActualizeazaButonAdauga()
        {
            if (preparat == null) return;

            cantitateText.Text = cantitate.ToString();
            decimal total = preparat.Pret * cantitate;
            adaugaText.Text = $"Adaugă în coș - {total:F2} RON";
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (cantitate > 1)
            {
                cantitate--;
                ActualizeazaButonAdauga();
            }
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            cantitate++;
            ActualizeazaButonAdauga();
        }

        private void Adauga_Click(object sender, RoutedEventArgs e)
        {
            if (preparat == null) return;

            CartSession.AdaugaPreparat(preparat, cantitate);

            this.Close();
        }

        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}