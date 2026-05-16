namespace RestaurantCarol.Layers
{
    public static class CodComandaGenerator
    {
        public static string GenereazaCod()
        {
            int numar = new ComandaDAL().GetUrmatorulNumarComanda();
            return numar.ToString();
        }
    }
}
