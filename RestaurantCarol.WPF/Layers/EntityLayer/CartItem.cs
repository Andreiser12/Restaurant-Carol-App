namespace RestaurantCarol.Layers
{
    public class CartItem : BasePropertyChanged
    {
        private Preparat? preparat;
        public Preparat? Preparat
        {
            get => preparat;
            set
            {
                preparat = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Denumire));
                NotifyPropertyChanged(nameof(PretUnitar));
                NotifyPropertyChanged(nameof(Subtotal));
                NotifyPropertyChanged(nameof(CalePoza));
            }
        }

        private Meniu? meniu;
        public Meniu? Meniu
        {
            get => meniu;
            set
            {
                meniu = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Denumire));
                NotifyPropertyChanged(nameof(PretUnitar));
                NotifyPropertyChanged(nameof(Subtotal));
                NotifyPropertyChanged(nameof(CalePoza));
            }
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
        public bool EsteMeniu => Meniu != null;
        public string Denumire => Preparat?.Denumire ?? Meniu?.Denumire ?? "Necunoscut";
        public decimal PretUnitar => Preparat?.Pret ?? Meniu?.Pret ?? 0;
        public decimal Subtotal => PretUnitar * Cantitate;
        public string? CalePoza => EsteMeniu
            ? Meniu.CalePozaImplicita
            : (string.IsNullOrEmpty(Preparat?.PrimaCalePoza) ? "/Images/carol_logo.png" : Preparat.PrimaCalePoza);
    }
}
