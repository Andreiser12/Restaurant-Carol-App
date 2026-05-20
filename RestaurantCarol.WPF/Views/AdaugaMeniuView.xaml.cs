using System.Windows;
using System.Windows.Input;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class AdaugaMeniuView : Window
    {
        private AdaugaMeniuViewModel viewModel;

        public AdaugaMeniuView()
        {
            InitializeComponent();

            viewModel = new AdaugaMeniuViewModel();
            DataContext = viewModel;

            viewModel.InchideRequested += succes =>
            {
                DialogResult = succes;
                Close();
            };

            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    DragMove();
            };
        }
    }
}