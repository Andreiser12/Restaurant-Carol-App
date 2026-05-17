namespace RestaurantCarol.Layers
{
    public class Adresa : BasePropertyChanged
    {
        private int idAdresa;
        public int IdAdresa
        {
            get => idAdresa;
            set { idAdresa = value; NotifyPropertyChanged(); }
        }
        private int idUtilizator;
        public int IdUtilizator
        {
            get => idUtilizator;
            set { idUtilizator = value; NotifyPropertyChanged(); }
        }
        private string adresaText = string.Empty;
        public string AdresaText
        {
            get => adresaText;
            set { adresaText = value; NotifyPropertyChanged(); }
        }
        private bool esteImplicita;
        public bool EsteImplicita
        {
            get => esteImplicita;
            set { esteImplicita = value; NotifyPropertyChanged(); }
        }
    }
}