using System;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AngajatHubUserControlViewModel : ViewModelBase
    {
        private Action<string> navigateAction;

        public bool IsBucatarEnabled => UserSession.CurrentUser != null && UserSession.IsBucatar;
        public double BucatarOpacity => IsBucatarEnabled ? 1.0 : 0.3;

        public bool IsManagerEnabled => UserSession.CurrentUser != null && UserSession.IsManager;
        public double ManagerOpacity => IsManagerEnabled ? 1.0 : 0.3;

        public bool IsLivratorEnabled => UserSession.CurrentUser != null && UserSession.IsLivrator;
        public double LivratorOpacity => IsLivratorEnabled ? 1.0 : 0.3;

        public ICommand BucatarCommand { get; }
        public ICommand ManagerCommand { get; }
        public ICommand LivratorCommand { get; }

        public AngajatHubUserControlViewModel(Action<string> navigateAction)
        {
            this.navigateAction = navigateAction;

            BucatarCommand = new RelayCommand<object>(_ => navigateAction("bucatar"));
            ManagerCommand = new RelayCommand<object>(_ => navigateAction("manager"));
            LivratorCommand = new RelayCommand<object>(_ => navigateAction("livrator"));
        }
    }
}
