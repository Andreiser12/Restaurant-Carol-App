namespace RestaurantCarol.Layers
{
    public class CartItem : BasePropertyChanged
    {
        private Preparat? preparat;
        public Preparat? Preparat
        {
            get => preparat;
            set { preparat = value; NotifyPropertyChanged(); }
        }

        private Meniu? meniu;
        public Meniu? Meniu
        {
            get => meniu;
            set { meniu = value; NotifyPropertyChanged(); }
        }

        private int cantitate;
        public int Cantitate
        {
            get => cantitate;
            set
            {
                cantitate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Subtotal));
            }
        }

        public string Denumire => Preparat?.Denumire ?? Meniu?.Denumire ?? "Necunoscut";

        public decimal PretUnitar => Preparat?.Pret ?? 0;

        public decimal Subtotal => PretUnitar * Cantitate;

        public string? CalePoza => Preparat?.PrimaCalePoza;
    }
}