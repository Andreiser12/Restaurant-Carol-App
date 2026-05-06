using RestaurantCarol.Layers;

namespace RestaurantCarol.Layers
{
    public class Categorie : BasePropertyChanged
    {
        private int _idCategorie;
        private string _denumire = string.Empty;

        public int IdCategorie
        {
            get => _idCategorie;
            set
            {
                if (_idCategorie != value)
                {
                    _idCategorie = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Denumire
        {
            get => _denumire;
            set
            {
                if (_denumire != value)
                {
                    _denumire = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TipCategorie tip;
        public TipCategorie Tip
        {
            get => tip;
            set { tip = value; NotifyPropertyChanged(); }
        }
    }
}


