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
            comboCategorie.ItemsSource = categorieBLL.GetAllCategorii();
            if (comboCategorie.Items.Count > 0)
                comboCategorie.SelectedIndex = 0;

            preparateList.Items.Clear();
            checkboxes.Clear();

            foreach (var p in preparatBLL.GetAllPreparate())
            {
                var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
                var cb = new CheckBox { Content = p.Denumire, Tag = p, VerticalAlignment = VerticalAlignment.Center };
                var txt = new TextBox { Width = 60, Margin = new Thickness(8, 0, 0, 0), Text = p.CantitatePortie.ToString() };
                panel.Children.Add(cb);
                panel.Children.Add(new TextBlock { Text = "g", VerticalAlignment = VerticalAlignment.Center });
                panel.Children.Add(txt);
                txt.Tag = p;
                preparateList.Items.Add(panel);
                checkboxes.Add(cb);
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
                    var txt = sp.Children.OfType<TextBox>().FirstOrDefault();
                    if (cb?.IsChecked == true && cb.Tag is Preparat prep && txt != null)
                    {
                        if (!decimal.TryParse(txt.Text, out decimal gramaj) || gramaj <= 0)
                            throw new RestaurantException($"Gramaj invalid pentru {prep.Denumire}.");
                        componente.Add(new MeniuPreparatItem
                        {
                            IdPreparat = prep.IdPreparat,
                            CantitatePortie = gramaj,
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
