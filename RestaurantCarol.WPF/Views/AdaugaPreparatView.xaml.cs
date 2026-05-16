using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Helpers;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class AdaugaPreparatView : Window
    {
        private PreparatBLL preparatBLL = new PreparatBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();

        private ObservableCollection<Alergen> totiAlergenii = new();
        private ObservableCollection<Categorie> categoriiMancare = new();
        private ObservableCollection<Categorie> categoriiBautura = new();

        private string? calePozaSursa;

        public AdaugaPreparatView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };

            IncarcaDateInitiale();
        }

        private void IncarcaDateInitiale()
        {
            try
            {
                totiAlergenii = preparatBLL.GetAllAlergeni();
                alergeniList.ItemsSource = totiAlergenii;

                categoriiMancare = categorieBLL.GetCategoriiByTip(TipCategorie.Mancare);
                categoriiBautura = categorieBLL.GetCategoriiByTip(TipCategorie.Bauturi);

                comboCategorie.ItemsSource = categoriiMancare;
                if (categoriiMancare.Count > 0)
                    comboCategorie.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare date: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabPreparat_Click(object sender, RoutedEventArgs e)
        {
            tabPreparat.Style = (Style)FindResource("TabButtonActive");
            tabCategorie.Style = (Style)FindResource("TabButton");
            formPreparat.Visibility = Visibility.Visible;
            formCategorie.Visibility = Visibility.Collapsed;
            btnSubmit.Content = "Adauga preparat";
        }

        private void TabCategorie_Click(object sender, RoutedEventArgs e)
        {
            tabCategorie.Style = (Style)FindResource("TabButtonActive");
            tabPreparat.Style = (Style)FindResource("TabButton");
            formCategorie.Visibility = Visibility.Visible;
            formPreparat.Visibility = Visibility.Collapsed;
            btnSubmit.Content = "Adauga categorie";
        }

        private void TipPreparat_Changed(object sender, RoutedEventArgs e)
        {
            if (comboCategorie == null) return;

            if (radioMancare.IsChecked == true)
            {
                comboCategorie.ItemsSource = categoriiMancare;
                if (categoriiMancare.Count > 0)
                    comboCategorie.SelectedIndex = 0;
            }
            else if (radioBautura.IsChecked == true)
            {
                comboCategorie.ItemsSource = categoriiBautura;
                if (categoriiBautura.Count > 0)
                    comboCategorie.SelectedIndex = 0;
            }
        }

        private void AlegePoza_Click(object sender, RoutedEventArgs e)
        {
            string? cale = ImageUploadHelper.AlegePoza();
            if (cale == null) return;

            try
            {
                calePozaSursa = cale;
                txtCalePoza.Text = System.IO.Path.GetFileName(cale);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(cale, UriKind.Absolute);
                bitmap.EndInit();

                previewImage.ImageSource = bitmap;
                previewBorder.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nu pot incarca poza: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                calePozaSursa = null;
                txtCalePoza.Text = "Nicio poza selectata";
                previewBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (formPreparat.Visibility == Visibility.Visible)
            {
                SubmitPreparat();
            }
            else if (formCategorie.Visibility == Visibility.Visible)
            {
                SubmitCategorie();
            }
        }

        private void SubmitCategorie()
        {
            try
            {
                Categorie cat = new Categorie
                {
                    Denumire = txtCatDenumire.Text,
                    Tip = radioCatMancare.IsChecked == true
                        ? TipCategorie.Mancare
                        : TipCategorie.Bauturi
                };

                categorieBLL.AddCategorie(cat);

                MessageBox.Show($"Categoria '{cat.Denumire}' a fost adaugata cu succes!",
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (RestaurantException ex)
            {
                MessageBox.Show(ex.Message, "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare neasteptata: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubmitPreparat()
        {
            try
            {
                if (comboCategorie.SelectedItem is not Categorie catSelectata)
                {
                    throw new RestaurantException("Selecteaza o categorie.");
                }

                decimal pret = ParseDecimal(txtPret.Text, "Pret");
                int cantitatePortie = ParseInt(txtCantitatePortie.Text, "Gramaj per portie");

                int? calorii = ParseOptionalInt(txtCalorii.Text, "Calorii");
                decimal? grasimi = ParseOptionalDecimal(txtGrasimi.Text, "Grasimi");
                decimal? carbohidrati = ParseOptionalDecimal(txtCarbohidrati.Text, "Carbohidrati");
                decimal? proteine = ParseOptionalDecimal(txtProteine.Text, "Proteine");
                decimal? sare = ParseOptionalDecimal(txtSare.Text, "Sare");

                Preparat preparat = new Preparat
                {
                    Denumire = txtDenumire.Text,
                    Pret = pret,
                    CantitatePortie = cantitatePortie,
                    CantitateTotala = 0,
                    Descriere = string.IsNullOrWhiteSpace(txtDescriere.Text)
                        ? null : txtDescriere.Text,
                    Calorii = calorii,
                    Grasimi = grasimi,
                    Carbohidrati = carbohidrati,
                    Proteine = proteine,
                    Sare = sare,
                    IdCategorie = catSelectata.IdCategorie
                };

                List<int> idsAlergeni = ExtrageAlergeniiBifati();

                string? caleFotografieRelativa = null;
                if (!string.IsNullOrEmpty(calePozaSursa))
                {
                    string prefix = SanitizeazaNume(preparat.Denumire);
                    caleFotografieRelativa = ImageUploadHelper.CopiazaPozaInRuntime(
                        calePozaSursa, prefix);
                }

                preparatBLL.AddPreparat(preparat, idsAlergeni, caleFotografieRelativa);

                MessageBox.Show($"Preparatul '{preparat.Denumire}' a fost adaugat cu succes!",
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (RestaurantException ex)
            {
                MessageBox.Show(ex.Message, "Validare",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare neasteptata: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<int> ExtrageAlergeniiBifati()
        {
            List<int> ids = new List<int>();

            for (int i = 0; i < alergeniList.Items.Count; i++)
            {
                var container = alergeniList.ItemContainerGenerator.ContainerFromIndex(i);
                if (container == null) continue;

                CheckBox? cb = FindVisualChild<CheckBox>(container);
                if (cb != null && cb.IsChecked == true && cb.Tag is int idAlergen)
                {
                    ids.Add(idAlergen);
                }
            }

            return ids;
        }

        private T? FindVisualChild<T>(System.Windows.DependencyObject parent)
            where T : System.Windows.DependencyObject
        {
            int count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T result) return result;

                var found = FindVisualChild<T>(child);
                if (found != null) return found;
            }
            return null;
        }


        private decimal ParseDecimal(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new RestaurantException($"Campul '{camp}' este obligatoriu.");

            text = text.Trim().Replace(",", ".");

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture,
                                  out decimal val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar valid.");

            return val;
        }

        private int ParseInt(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new RestaurantException($"Campul '{camp}' este obligatoriu.");

            if (!int.TryParse(text.Trim(), out int val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar intreg.");

            return val;
        }

        private decimal? ParseOptionalDecimal(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            text = text.Trim().Replace(",", ".");

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture,
                                  out decimal val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar valid sau gol.");

            return val;
        }

        private int? ParseOptionalInt(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            if (!int.TryParse(text.Trim(), out int val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar intreg sau gol.");

            return val;
        }

        private string SanitizeazaNume(string nume)
        {
            var caractereAcceptate = nume.ToLower()
                .Replace(" ", "_")
                .Where(c => char.IsLetterOrDigit(c) || c == '_')
                .ToArray();

            string rezultat = new string(caractereAcceptate);

            if (string.IsNullOrEmpty(rezultat))
                rezultat = "preparat";

            return rezultat;
        }

        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}