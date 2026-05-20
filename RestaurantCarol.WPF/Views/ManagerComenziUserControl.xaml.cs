using System.Windows.Controls;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class ManagerComenziUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private ManagerComenziViewModel viewModel;

        public ManagerComenziUserControl(AngajatHubView parent, bool activeOnly)
        {
            InitializeComponent();
            parentView = parent;

            viewModel = new ManagerComenziViewModel(activeOnly);
            DataContext = viewModel;

            viewModel.InapoiRequested += () => parentView?.NavigateToManager();
        }
    }
}