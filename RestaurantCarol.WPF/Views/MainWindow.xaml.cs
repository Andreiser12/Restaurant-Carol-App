
using RestaurantCarol.Views;
using System.Windows;

namespace RestaurantCarol.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void VeziCategorii_Click(object sender, RoutedEventArgs e)
    {
        CategoriiView window = new CategoriiView();
        window.Show();
    }
}
