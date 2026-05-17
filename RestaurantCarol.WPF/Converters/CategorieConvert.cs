using System.Globalization;
using System.Windows.Data;
using RestaurantCarol.Layers;
namespace RestaurantCarol.Converters
{
    public class CategorieConvert : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
                return null;
            if (values[0] == null)
                return null;
            return new Categorie
            {
                Denumire = values[0].ToString() ?? string.Empty
            };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}