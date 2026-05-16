namespace RestaurantCarol.Layers
{
    public static class LivratorSession
    {
        public static Comanda? ComandaSelectata { get; set; }

        public static void Reset()
        {
            ComandaSelectata = null;
        }
    }
}
