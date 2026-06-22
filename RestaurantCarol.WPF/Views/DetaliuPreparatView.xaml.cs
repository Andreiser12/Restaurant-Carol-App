using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RestaurantCarol.Helpers;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;
namespace RestaurantCarol.Views
{
    public partial class DetaliuPreparatView : Window
    {
        private DetaliuPreparatViewModel viewModel;
        public DetaliuPreparatView(Preparat preparat)
        {
            InitializeComponent();
            viewModel = new DetaliuPreparatViewModel(preparat);
            DataContext = viewModel;
            viewModel.InchideRequested += Close;
            IncarcaPoza();
            denumireText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.Denumire)));
            pretText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.PretText)));
            cantitatePortieText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.CantitatePortieText)));
            descriereText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.Descriere)));
            adaugaText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.AdaugaText)));
            cantitateText.SetBinding(TextBlock.TextProperty, new Binding(nameof(DetaliuPreparatViewModel.Cantitate)));
            alergeniList.ItemsSource = viewModel.Alergeni;
            cosSection.Visibility = viewModel.PoateAdaugaInCos ? Visibility.Visible : Visibility.Collapsed;
            oaspeteSection.Visibility = viewModel.PoateAdaugaInCos ? Visibility.Collapsed : Visibility.Visible;
            ingredienteSection.Visibility = viewModel.AreDescriere ? Visibility.Visible : Visibility.Collapsed;
            alergeniSection.Visibility = viewModel.Alergeni.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            if (viewModel.AreValoriNutritionale)
            {
                valoriSection.Visibility = Visibility.Visible;
                if (viewModel.Calorii.HasValue) valoriList.Children.Add(new TextBlock { Text = $"Calorii: {viewModel.Calorii} kcal" });
                if (viewModel.Grasimi.HasValue) valoriList.Children.Add(new TextBlock { Text = $"Grăsimi: {viewModel.Grasimi}g" });
                if (viewModel.Carbohidrati.HasValue) valoriList.Children.Add(new TextBlock { Text = $"Carbohidrați: {viewModel.Carbohidrati}g" });
                if (viewModel.Proteine.HasValue) valoriList.Children.Add(new TextBlock { Text = $"Proteine: {viewModel.Proteine}g" });
                if (viewModel.Sare.HasValue) valoriList.Children.Add(new TextBlock { Text = $"Sare: {viewModel.Sare}g" });
            }
            else
            {
                valoriSection.Visibility = Visibility.Collapsed;
            }
            MouseLeftButtonDown += (_, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed) DragMove();
            };
        }

        private void IncarcaPoza()
        {
            try
            {
                string path = ImageUploadHelper.ConstruiestePathPentruImage(viewModel.CalePoza);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.EndInit();
                pozaPreparat.ImageSource = bitmap;
            }
            catch { }
        }

        private void Inchide_Click(object sender, RoutedEventArgs e) => viewModel.InchideCommand.Execute(null);
        private void Minus_Click(object sender, RoutedEventArgs e) => viewModel.MinusCommand.Execute(null);
        private void Plus_Click(object sender, RoutedEventArgs e) => viewModel.PlusCommand.Execute(null);
        private void Adauga_Click(object sender, RoutedEventArgs e) => viewModel.AdaugaCommand.Execute(null);
    }
}
