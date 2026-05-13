namespace RestaurantCarol.Layers
{
    public class Utilizator : BasePropertyChanged
    {
        private int idUtilizator;
        public int IdUtilizator
        {
            get => idUtilizator;
            set { idUtilizator = value; NotifyPropertyChanged(); }
        }

        private string nume = string.Empty;
        public string Nume
        {
            get => nume;
            set { nume = value; NotifyPropertyChanged(); }
        }

        private string prenume = string.Empty;
        public string Prenume
        {
            get => prenume;
            set { prenume = value; NotifyPropertyChanged(); }
        }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; NotifyPropertyChanged(); }
        }

        private string? telefon;
        public string? Telefon
        {
            get => telefon;
            set { telefon = value; NotifyPropertyChanged(); }
        }

        private string parolaHash = string.Empty;
        public string ParolaHash
        {
            get => parolaHash;
            set { parolaHash = value; NotifyPropertyChanged(); }
        }

        private RolUtilizator rol = RolUtilizator.Client;
        public RolUtilizator Rol
        {
            get => rol;
            set { rol = value; NotifyPropertyChanged(); }
        }
    }
}