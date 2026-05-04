namespace RestaurantCarol.Layers
{
    public static class UserSession
    {
        public static Utilizator? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsClient => CurrentUser?.RolUtilizator == RolUtilizator.Client;

        public static bool IsAngajat => CurrentUser?.RolUtilizator == RolUtilizator.Angajat;

        public static void Login(Utilizator utilizator)
        {
            CurrentUser = utilizator;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}