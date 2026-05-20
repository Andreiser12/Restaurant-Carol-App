using System.Windows;
using System.Windows.Controls;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
using RestaurantCarol.Views.Navigation;

namespace RestaurantCarol.Views
{
    public partial class HubUserControl : UserControl
    {
        private HubViewModel? viewModel;
        private IMeniuRestaurantNavigator? parentView;

        public HubUserControl()
        {
            InitializeComponent();
        }

        public HubUserControl(IMeniuRestaurantNavigator parent) : this()
        {
            parentView = parent;
            viewModel = new HubViewModel(parent);
            DataContext = viewModel;
            viewModel.DeschideDetaliuPreparatCerere += DeschideDetaliuPreparat;
        }

        private void DeschideDetaliuPreparat(Preparat preparat)
        {
            var popup = new DetaliuPreparatView(preparat)
            {
                Owner = parentView?.GetHostWindow()
            };
            popup.ShowDialog();
        }
    }
}