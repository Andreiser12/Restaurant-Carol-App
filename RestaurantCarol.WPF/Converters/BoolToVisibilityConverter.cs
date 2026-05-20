using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace RestaurantCarol.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Visibility v && v == Visibility.Visible)
                return true;
            return false;
        }
    }

    public class BoolToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Visibility v && v == Visibility.Collapsed)
                return true;
            return false;
        }
    }
}