namespace RestaurantCarol.Layers
{
    public class MeniuPreparatItem
    {
        public int IdMeniu { get; set; }
        public int IdPreparat { get; set; }
        public decimal CantitatePortie { get; set; }
        public Preparat? Preparat { get; set; }
        public string DenumirePreparat => Preparat?.Denumire ?? string.Empty;
        public decimal PretPreparat => Preparat?.Pret ?? 0;
    }
}
