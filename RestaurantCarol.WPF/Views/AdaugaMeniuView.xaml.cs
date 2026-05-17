using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;
namespace RestaurantCarol.Views
{
    public partial class AdaugaMeniuView : Window
    {
        private MeniuBLL meniuBLL = new MeniuBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();
        private PreparatBLL preparatBLL = new PreparatBLL();
        private List<CheckBox> checkboxes = new();
        public AdaugaMeniuView()
        {
            InitializeComponent();
            IncarcaDate();
            MouseLeftButtonDown += (_, e) => { if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed) DragMove(); };
        }
        private void IncarcaDate()
        {
            try
            {
                comboCategorie.SelectionChanged += ComboCategorie_SelectionChanged;
                comboCategorie.ItemsSource = categorieBLL.GetAllCategorii();
                if (comboCategorie.Items.Count > 0)
                    comboCategorie.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea datelor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ComboCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                preparateList.Items.Clear();
                checkboxes.Clear();
                if (comboCategorie.SelectedItem is Categorie cat)
                {
                    foreach (var p in preparatBLL.GetAllPreparate().Where(prep => prep.IdCategorie == cat.IdCategorie))
                    {
                        var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
                        var cb = new CheckBox { Content = p.Denumire, Tag = p, VerticalAlignment = VerticalAlignment.Center };
                        panel.Children.Add(cb);
                        preparateList.Items.Add(panel);
                        checkboxes.Add(cb);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea datelor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Salveaza_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboCategorie.SelectedItem is not Categorie cat)
                    throw new RestaurantException("Selecteaza o categorie.");
                var componente = new List<MeniuPreparatItem>();
                foreach (var panel in preparateList.Items)
                {
                    if (panel is not StackPanel sp) continue;
                    var cb = sp.Children.OfType<CheckBox>().FirstOrDefault();
                    if (cb?.IsChecked == true && cb.Tag is Preparat prep)
                    {
                        componente.Add(new MeniuPreparatItem
                        {
                            IdPreparat = prep.IdPreparat,
                            CantitatePortie = prep.CantitatePortie,
                            Preparat = prep
                        });
                    }
                }
                meniuBLL.AddMeniu(txtDenumire.Text.Trim(), cat.IdCategorie, componente);
                MessageBox.Show("Meniul a fost adaugat.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (RestaurantException ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Anuleaza_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
