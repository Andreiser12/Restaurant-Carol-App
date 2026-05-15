namespace RestaurantCarol.Layers
{
    public static class UserSession
    {
        public static Utilizator? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsClient => CurrentUser?.Rol == RolUtilizator.Client;
        public static bool IsBucatar => CurrentUser?.Rol == RolUtilizator.Bucatar;
        public static bool IsManager => CurrentUser?.Rol == RolUtilizator.Manager;
        public static bool IsLivrator => CurrentUser?.Rol == RolUtilizator.Livrator;

        public static bool IsAngajat => IsBucatar || IsManager || IsLivrator;

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