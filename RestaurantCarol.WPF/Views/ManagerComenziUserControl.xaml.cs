using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class ManagerComenziUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private ComandaBLL comandaBLL = new ComandaBLL();
        private bool doarActive;

        public List<StareComanda> StariDisponibile { get; set; }

        public ManagerComenziUserControl(AngajatHubView parent, bool activeOnly)
        {
            InitializeComponent();
            parentView = parent;
            doarActive = activeOnly;

            titleText.Text = doarActive ? "Comenzi Active" : "Toate Comenzile";

            StariDisponibile = Enum.GetValues(typeof(StareComanda)).Cast<StareComanda>().ToList();

            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                ObservableCollection<Comanda> comenzi = comandaBLL.GetComenziManager(doarActive);
                comenziList.ItemsSource = comenzi;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea comenzilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToManager();
        }

        private void AplicaStare_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Comanda comanda)
            {
                // Find the parent Grid, then the ComboBox
                var parentGrid = btn.Parent as Grid;
                if (parentGrid != null)
                {
                    var comboBox = parentGrid.Children.OfType<ComboBox>().FirstOrDefault();
                    if (comboBox != null && comboBox.SelectedValue != null)
                    {
                        StareComanda nouaStare = (StareComanda)comboBox.SelectedValue;

                        // Valideaza daca starea este finala
                        if (comanda.StareComanda == StareComanda.Livrata || comanda.StareComanda == StareComanda.Anulata)
                        {
                            MessageBox.Show("Aceasta comanda este intr-o stare finala si nu mai poate fi modificata.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        try
                        {
                            comandaBLL.UpdateStareComanda(comanda.IdComanda, nouaStare);
                            MessageBox.Show("Starea comenzii a fost actualizata cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // Reload to reflect changes (if it was active and now it's Livrata, it should disappear from Active)
                            IncarcaDate();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"A aparut o eroare la salvare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
