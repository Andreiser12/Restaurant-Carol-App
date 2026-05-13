namespace RestaurantCarol.Layers
{
    public static class UserSession
    {
        public static Utilizator? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsClient => CurrentUser?.Rol == RolUtilizator.Client;

        public static bool IsAngajat => CurrentUser?.Rol == RolUtilizator.Angajat;

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