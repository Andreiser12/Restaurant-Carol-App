using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class StareComandaListView : Window
    {
        public event Action? ComenziActualizate;

        private StareComandaListViewModel viewModel;

        public StareComandaListView()
        {
            InitializeComponent();

            viewModel = new StareComandaListViewModel();
            DataContext = viewModel;

            viewModel.InchideRequested += Close;
            viewModel.DeschideDetalii += DeschideDetalii;

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
                ComenziActualizate?.Invoke();
            };
            detalii.ShowDialog();
        }
    }
}