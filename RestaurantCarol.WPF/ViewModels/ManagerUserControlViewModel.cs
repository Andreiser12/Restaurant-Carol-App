using System;
using System.Windows.Input;
using RestaurantCarol.Commands;

namespace RestaurantCarol.ViewModels
{
    public class ManagerUserControlViewModel : ViewModelBase
    {
        private Action<string> navigateAction;

        public ICommand InapoiCommand { get; }
        public ICommand ToateComenzileCommand { get; }
        public ICommand ComenziActiveCommand { get; }
        public ICommand StocRedusCommand { get; }

        public ManagerUserControlViewModel(Action<string> navigateAction)
        {
            this.navigateAction = navigateAction;

            InapoiCommand = new RelayCommand<object>(_ => navigateAction("inapoi"));
            ToateComenzileCommand = new RelayCommand<object>(_ => navigateAction("toateComenzile"));
            ComenziActiveCommand = new RelayCommand<object>(_ => navigateAction("comenziActive"));
            StocRedusCommand = new RelayCommand<object>(_ => navigateAction("stocRedus"));
        }
    }
}
