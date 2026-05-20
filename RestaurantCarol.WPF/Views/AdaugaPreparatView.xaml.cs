using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RestaurantCarol.Helpers;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class AdaugaPreparatView : Window
    {
        public event Action? PreparatModificat;

        private AdaugaPreparatViewModel viewModel;

        public AdaugaPreparatView() : this(viewModelMaker: () => new AdaugaPreparatViewModel())
        {
        }

        public AdaugaPreparatView(int idPreparat)
            : this(viewModelMaker: () => new AdaugaPreparatViewModel(idPreparat))
        {
        }

        private AdaugaPreparatView(Func<AdaugaPreparatViewModel> viewModelMaker)
        {
            InitializeComponent();

            viewModel = viewModelMaker();
            DataContext = viewModel;

            viewModel.InchideRequested += () => Close();
            viewModel.PreparatModificat += () => PreparatModificat?.Invoke();
            viewModel.AfiseazaPreviewPozaCerere += AfiseazaPreviewPoza;
            viewModel.AscundePreviewPozaCerere += AscundePreviewPoza;

            MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    DragMove();
            };
        }

        private void AfiseazaPreviewPoza(string calePoza)
        {
            try
            {
                string pathFinal = calePoza.StartsWith("/") || System.IO.File.Exists(calePoza)
                    ? (System.IO.File.Exists(calePoza) ? calePoza
                        : ImageUploadHelper.ConstruiestePathPentruImage(calePoza))
                    : ImageUploadHelper.ConstruiestePathPentruImage(calePoza);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(pathFinal, UriKind.Absolute);
                bitmap.EndInit();

                previewImage.ImageSource = bitmap;
                previewBorder.Visibility = Visibility.Visible;
            }
            catch
            {
                previewBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void AscundePreviewPoza()
        {
            previewImage.ImageSource = null;
            previewBorder.Visibility = Visibility.Collapsed;
        }
    }
}