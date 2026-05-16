using System.Configuration;

namespace RestaurantCarol.Layers
{
    public static class AppSettingsHelper
    {
        public static decimal GetDecimal(string key, decimal defaultValue)
        {
            string? raw = ConfigurationManager.AppSettings[key];
            return decimal.TryParse(raw, out decimal value) ? value : defaultValue;
        }

        public static int GetInt(string key, int defaultValue)
        {
            string? raw = ConfigurationManager.AppSettings[key];
            return int.TryParse(raw, out int value) ? value : defaultValue;
        }

        public static decimal DiscountMeniu => GetDecimal("DiscountMeniu", 10);
        public static decimal PragDiscountSuma => GetDecimal("PragDiscountSuma", 200);
        public static int NrComenziDiscount => GetInt("NrComenziDiscount", 3);
        public static int IntervalZileDiscount => GetInt("IntervalZileDiscount", 30);
        public static decimal ProcentDiscount => GetDecimal("ProcentDiscount", 5);
        public static decimal PragTransportGratuit => GetDecimal("PragTransportGratuit", 100);
        public static decimal CostTransport => GetDecimal("CostTransport", 15);
        public static int PragStocRedus => GetInt("PragStocRedus", 500);
    }
}
