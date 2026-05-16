using RestaurantCarol.Layers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantCarol.Views
{
    public partial class HubUserControl : UserControl
    {
        private MeniuRestaurantView? parentView;
        private PreparatBLL preparatBLL = new PreparatBLL();
        private AlergenBLL alergenBLL = new AlergenBLL();
        private ObservableCollection<Preparat> totalPreparate = [];
        private List<int> alergeniDeEvitat = new List<int>();
        private bool alergeniPanouDeschis = false;

        public HubUserControl()
        {
            InitializeComponent();
        }

        public HubUserControl(MeniuRestaurantView parent) : this()
        {
            parentView = parent;
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            { 
                totalPreparate = preparatBLL.GetAllPreparate();

                var alergeni = alergenBLL.GetAllAlergeni();
                alergeniFiltruList.ItemsSource = alergeni;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare date: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AplicaFiltre()
        {
            string textCautare = cautareTextBox.Text.Trim().ToLower();
            bool esteSearchActiv = !string.IsNullOrEmpty(textCautare) || alergeniDeEvitat.Count > 0;

            if (esteSearchActiv)
            {
                opresteCautareaButton.Visibility = Visibility.Visible;
                cercuriNavigare.Visibility = Visibility.Collapsed;
                rezultateContainer.Visibility = Visibility.Visible;

                var rezultate = totalPreparate.Where(p =>
                {
                    bool potriveste = string.IsNullOrEmpty(textCautare)
                        || p.Denumire.ToLower().Contains(textCautare);

                    if (!potriveste) return false;

                    foreach (int idAlergen in alergeniDeEvitat)
                    {
                        string cautare = $",{idAlergen},";
                        if (p.IdsAlergeni.Contains(cautare))
                        {
                            return false;
                        }
                    }

                    return true;
                }).ToList();

                rezultateList.ItemsSource = rezultate;

                if (rezultate.Count == 0)
                {
                    mesajGolBorder.Visibility = Visibility.Visible;
                    rezultateList.Visibility = Visibility.Collapsed;

                    if (!string.IsNullOrEmpty(textCautare) && alergeniDeEvitat.Count > 0)
                    {
                        mesajGolText.Text = $"Nu am gasit preparate cu '{cautareTextBox.Text}' care sa nu contina alergenii selectati.";
                    }
                    else if (!string.IsNullOrEmpty(textCautare))
                    {
                        mesajGolText.Text = $"Nu am gasit preparate cu '{cautareTextBox.Text}'.";
                    }
                    else
                    {
                        mesajGolText.Text = "Nu am gasit preparate fara alergenii selectati.";
                    }
                }
                else
                {
                    mesajGolBorder.Visibility = Visibility.Collapsed;
                    rezultateList.Visibility = Visibility.Visible;
                }
            }
            else
            {
                opresteCautareaButton.Visibility = Visibility.Collapsed;
                cercuriNavigare.Visibility = Visibility.Visible;
                rezultateContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void CautareTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicaFiltre();
        }

        private void FiltreazaAlergeni_Click(object sender, RoutedEventArgs e)
        {
            alergeniPanouDeschis = !alergeniPanouDeschis;

            if (alergeniPanouDeschis)
            {
                alergeniPanel.Visibility = Visibility.Visible;
                filtreazaAlergeniButton.Content = "▲ Filtreaza alergeni";
            }
            else
            {
                alergeniPanel.Visibility = Visibility.Collapsed;
                filtreazaAlergeniButton.Content = "▼ Filtreaza alergeni";
            }
        }

        private void AlergenFiltru_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is Alergen alergen)
            {
                if (cb.IsChecked == true)
                {
                    if (!alergeniDeEvitat.Contains(alergen.IdAlergen))
                        alergeniDeEvitat.Add(alergen.IdAlergen);
                }
                else
                {
                    alergeniDeEvitat.Remove(alergen.IdAlergen);
                }

                AplicaFiltre();
            }
        }

        private void OpresteCautarea_Click(object sender, RoutedEventArgs e)
        {
            cautareTextBox.Text = "";
            alergeniDeEvitat.Clear();

            foreach (var item in alergeniFiltruList.Items)
            {
                if (alergeniFiltruList.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement container)
                {
                    var checkBox = FindVisualChild<CheckBox>(container);
                    if (checkBox != null) checkBox.IsChecked = false;
                }
            }

            if (alergeniPanouDeschis)
            {
                alergeniPanouDeschis = false;
                alergeniPanel.Visibility = Visibility.Collapsed;
                filtreazaAlergeniButton.Content = "▼ Filtreaza alergeni";
            }

            AplicaFiltre();
        }

        private T? FindVisualChild<T>(System.Windows.DependencyObject parent) where T : System.Windows.DependencyObject
        {
            int count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T result) return result;
                var deep = FindVisualChild<T>(child);
                if (deep != null) return deep;
            }
            return null;
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

        private void Populare_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToListaPreparatePopulare();
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
            parentView?.NavigateToListaCategorii(
                TipCategorie.Bauturi,
                "Bauturi",
                "/Images/categorie_bauturi.png");
        }
    }
}