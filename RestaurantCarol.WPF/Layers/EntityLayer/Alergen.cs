namespace RestaurantCarol.Layers
{
    public class Alergen : BasePropertyChanged
    {
        private int idAlergen;
        public int IdAlergen
        {
            get => idAlergen;
            set { idAlergen = value; NotifyPropertyChanged(); }
        }

        private string denumire = string.Empty;
        public string Denumire
        {
            get => denumire;
            set { denumire = value; NotifyPropertyChanged(); }
        }
    }
}