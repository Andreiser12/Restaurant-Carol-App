using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using RestaurantCarol.Helpers;
namespace RestaurantCarol.Converters
{
    public class CalePozaConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string calePoza;
            if (value is string s && !string.IsNullOrWhiteSpace(s))
                calePoza = s;
            else
                calePoza = "/Images/carol_logo.png";
            try
            {
                string pathFinal = ImageUploadHelper.ConstruiestePathPentruImage(calePoza);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(pathFinal, UriKind.Absolute);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}