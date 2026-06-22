using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
namespace RestaurantCarol.Views
{
    public partial class DetaliuMeniuView : Window
    {
        private DetaliuMeniuViewModel viewModel;

        public DetaliuMeniuView(Meniu meniu)
        {
            InitializeComponent();
            viewModel = new DetaliuMeniuViewModel(meniu);
            DataContext = viewModel;
            viewModel.InchideRequested += Close;
            indisponibilText.Visibility = viewModel.EsteDisponibil ? Visibility.Collapsed : Visibility.Visible;
            cosSection.Visibility = viewModel.PoateAdaugaInCos ? Visibility.Visible : Visibility.Collapsed;
            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            };
        }
    }
}
