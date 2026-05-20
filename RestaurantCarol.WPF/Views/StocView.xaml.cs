using System.Windows;
using System.Windows.Input;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class StocView : Window
    {
        private StocViewModel viewModel;

        public StocView()
        {
            InitializeComponent();

            viewModel = new StocViewModel();
            DataContext = viewModel;

            viewModel.InchideRequested += () => Close();

            MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    DragMove();
            };
        }
    }
}