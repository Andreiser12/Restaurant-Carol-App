using System.Configuration;
using System.Data;
using System.Windows;
namespace RestaurantCarol.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("CRITICAL CRASH INFO:\n\n" + e.Exception.ToString(), "Eroare Fatala", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
