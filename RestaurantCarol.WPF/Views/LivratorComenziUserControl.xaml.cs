using System.Windows.Controls;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class LivratorComenziUserControl : UserControl
    {
        private AngajatHubView? parentView;
        private LivratorComenziViewModel viewModel;

        public LivratorComenziUserControl(AngajatHubView parent)
        {
            InitializeComponent();
            parentView = parent;

            viewModel = new LivratorComenziViewModel();
            DataContext = viewModel;

            viewModel.InapoiRequested += () => parentView?.NavigateToLivrator();
        }
    }
}