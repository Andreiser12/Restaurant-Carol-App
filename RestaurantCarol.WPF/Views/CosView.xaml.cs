using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
using RestaurantCarol.Views.Navigation;
namespace RestaurantCarol.Views
{
    public partial class CosView : Window
    {
        private CosViewModel viewModel;
        public CosView(IMeniuRestaurantNavigator? navigator = null)
        {
            InitializeComponent();
            viewModel = new CosViewModel(navigator);
            DataContext = viewModel;
            viewModel.InchideRequested += Close;
            viewModel.ComandaPlasata += Close;
            itemsList.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding(nameof(CosViewModel.Items)));
            adreseComboBox.SetBinding(ComboBox.ItemsSourceProperty,
                new Binding(nameof(CosViewModel.Adrese)));
            adreseComboBox.SetBinding(ComboBox.SelectedItemProperty,
                new Binding(nameof(CosViewModel.AdresaSelectata)) { Mode = BindingMode.TwoWay });
            totalText.SetBinding(TextBlock.TextProperty,
                new Binding(nameof(CosViewModel.Total)) { StringFormat = "{0:F2} RON" });
            CartSession.CartChanged += ActualizeazaVizibilitate;
            ActualizeazaVizibilitate();
            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed) DragMove();
            };
        }
        private void ActualizeazaVizibilitate()
        {
            bool gol = viewModel.EsteGol;
            itemsList.Visibility = gol ? Visibility.Collapsed : Visibility.Visible;
            cosGolText.Visibility = gol ? Visibility.Visible : Visibility.Collapsed;
            adresaSection.Visibility = gol ? Visibility.Collapsed : Visibility.Visible;
            plaseazaButton.IsEnabled = viewModel.PoatePlasa;
            plaseazaButton.Opacity = viewModel.PoatePlasa ? 1 : 0.5;
        }
        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button)?.Tag is Layers.CartItem item)
                viewModel.MinusCommand.Execute(item);
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button)?.Tag is Layers.CartItem item)
                viewModel.PlusCommand.Execute(item);
        }
        private void Sterge_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button)?.Tag is Layers.CartItem item)
                viewModel.StergeCommand.Execute(item);
        }
        private void Plaseaza_Click(object sender, RoutedEventArgs e) => viewModel.PlaseazaCommand.Execute(null);
        private void Inchide_Click(object sender, RoutedEventArgs e) => viewModel.InchideCommand.Execute(null);
        protected override void OnClosed(EventArgs e)
        {
            CartSession.CartChanged -= ActualizeazaVizibilitate;
            viewModel.Detach();
            base.OnClosed(e);
        }
    }
}
