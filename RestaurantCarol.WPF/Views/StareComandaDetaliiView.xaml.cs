using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
namespace RestaurantCarol.Views
{
    public partial class StareComandaDetaliiView : Window
    {
        private StareComandaDetaliiViewModel viewModel;
        public StareComandaDetaliiView(Comanda comanda)
        {
            InitializeComponent();
            viewModel = new StareComandaDetaliiViewModel(comanda);
            DataContext = viewModel;
            viewModel.InchideRequested += Close;
            titleText.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding(nameof(StareComandaDetaliiViewModel.Titlu)));
            dataText.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding(nameof(StareComandaDetaliiViewModel.DataText)));
            oraText.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding(nameof(StareComandaDetaliiViewModel.OraText)));
            totalText.SetBinding(TextBlock.TextProperty,
                new System.Windows.Data.Binding(nameof(StareComandaDetaliiViewModel.TotalText)));
            foreach (var linie in viewModel.LiniiProduse)
                produseList.Items.Add(new TextBlock { Text = linie, FontSize = 13, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, 2, 0, 2) });
            anulataText.Visibility = viewModel.EsteAnulata ? Visibility.Visible : Visibility.Collapsed;
            stariPanel.Visibility = viewModel.EsteAnulata ? Visibility.Collapsed : Visibility.Visible;
            btnAnuleaza.Visibility = viewModel.PoateAnula ? Visibility.Visible : Visibility.Collapsed;
            chkInregistrata.IsChecked = viewModel.PasInregistrata;
            chkSePregateste.IsChecked = viewModel.PasSePregateste;
            chkAPlecat.IsChecked = viewModel.PasAPlecat;
            chkLivrata.IsChecked = viewModel.PasLivrata;
            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed) DragMove();
            };
        }
        public event Action? ComandaActualizata;
        private void Anuleaza_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AnuleazaCommand.Execute(null);
            ComandaActualizata?.Invoke();
        }
        private void Inchide_Click(object sender, RoutedEventArgs e) =>
            viewModel.InchideCommand.Execute(null);
    }
}
