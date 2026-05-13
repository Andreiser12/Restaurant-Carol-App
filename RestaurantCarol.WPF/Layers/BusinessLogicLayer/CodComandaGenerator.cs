namespace RestaurantCarol.Layers
{
    public static class CodComandaGenerator
    {
        public static string GenereazaCod()
        {
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            return guid.Substring(0, 8);
        }
    }
}