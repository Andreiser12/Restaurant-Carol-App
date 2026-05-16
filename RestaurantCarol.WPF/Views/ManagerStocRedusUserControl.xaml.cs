using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class ManagerStocRedusUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private PreparatBLL preparatBLL = new PreparatBLL();

        public ManagerStocRedusUserControl(AngajatHubView parent)
        {
            InitializeComponent();
            parentView = parent;

            IncarcaDate();
        }

        private void IncarcaDate()
        {
            try
            {
                int prag = 500;
                string? pragStr = ConfigurationManager.AppSettings["PragStocRedus"];
                if (int.TryParse(pragStr, out int configPrag))
                {
                    prag = configPrag;
                }

                subtitleText.Text = $"Urmatoarele produse se apropie de epuizare (sub {prag} unitati):";

                ObservableCollection<Preparat> preparate = preparatBLL.GetPreparateStocRedus(prag);
                produseList.ItemsSource = preparate;

                if (preparate.Count == 0)
                {
                    subtitleText.Text = "Nu exista produse care se apropie de epuizare.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea stocului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inapoi_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToManager();
        }
    }
}
