using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
namespace RestaurantCarol.Views
{
    public partial class StareComandaListView : Window
    {
        private StareComandaListViewModel viewModel;
        public StareComandaListView()
        {
            InitializeComponent();
            viewModel = new StareComandaListViewModel();
            DataContext = viewModel;
            viewModel.InchideRequested += Close;
            viewModel.DeschideDetalii += DeschideDetalii;
            comenziList.ItemsSource = viewModel.Comenzi;
            emptyText.Visibility = viewModel.EsteGol ? Visibility.Visible : Visibility.Collapsed;
            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed) DragMove();
            };
        }
        private void DeschideDetalii(Comanda comanda)
        {
            var detalii = new StareComandaDetaliiView(comanda);
            detalii.Owner = this;
            detalii.ComandaActualizata += () =>
            {
                viewModel.Incarca();
                emptyText.Visibility = viewModel.EsteGol ? Visibility.Visible : Visibility.Collapsed;
                ComenziActualizate?.Invoke();
            };
            detalii.ShowDialog();
        }
        public event Action? ComenziActualizate;
        private void ComenziList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.Selectata = comenziList.SelectedItem as Comanda;
        }
        private void ComenziList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.DeschideSelectataCommand.Execute(null);
        }
        private void DeschideSelectata_Click(object sender, RoutedEventArgs e) =>
            viewModel.DeschideSelectataCommand.Execute(null);
        private void Inchide_Click(object sender, RoutedEventArgs e) =>
            viewModel.InchideCommand.Execute(null);
    }
}
