namespace RestaurantCarol.Models
{
    public class MeniuPreparatItem
    {
        public int IdMeniu { get; set; }
        public int IdPreparat { get; set; }
        public decimal CantitatePortie { get; set; }

        public Preparat? Preparat { get; set; }
    }
}