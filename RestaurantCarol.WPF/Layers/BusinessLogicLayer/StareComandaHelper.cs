namespace RestaurantCarol.Layers
{
    public static class StareComandaHelper
    {
        public static readonly StareComanda[] PasiUrmarire =
        {
            StareComanda.Inregistrata,
            StareComanda.SePregateste,
            StareComanda.APlecatLaClient,
            StareComanda.Livrata
        };
        public static bool EsteStareFinala(StareComanda stare) =>
            stare == StareComanda.Livrata || stare == StareComanda.Anulata;
        public static bool EstePasAtins(StareComanda stareCurenta, StareComanda pas)
        {
            if (stareCurenta == StareComanda.Anulata)
                return false;
            int idxCurent = Array.IndexOf(PasiUrmarire, stareCurenta);
            int idxPas = Array.IndexOf(PasiUrmarire, pas);
            if (idxCurent < 0 || idxPas < 0)
                return false;
            return idxCurent >= idxPas;
        }
        public static string GetDenumireAfisata(StareComanda stare) => stare switch
        {
            StareComanda.Inregistrata => "Inregistrata",
            StareComanda.SePregateste => "Se pregateste",
            StareComanda.APlecatLaClient => "A plecat la client",
            StareComanda.Livrata => "Livrata",
            StareComanda.Anulata => "Anulata",
            _ => stare.ToString()
        };
    }
}
