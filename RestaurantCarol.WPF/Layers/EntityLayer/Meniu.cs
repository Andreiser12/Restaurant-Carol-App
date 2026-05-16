using System.Linq;

namespace RestaurantCarol.Layers
{
    public class Meniu : BasePropertyChanged
    {
        public int IdMeniu { get; set; }
        public string Denumire { get; set; } = string.Empty;
        public int IdCategorie { get; set; }
        public Categorie? Categorie { get; set; }

        public List<MeniuPreparatItem> Componente { get; set; } = new();

        public decimal Pret { get; set; }

        public bool EsteDisponibil { get; set; }

        public string GramajeAfisate
        {
            get
            {
                if (Componente.Count == 0) return string.Empty;
                return string.Join(" / ", Componente.Select(c =>
                    $"{c.DenumirePreparat} {c.CantitatePortie:0}g"));
            }
        }

        public const string CalePozaImplicita = "/Images/carol_logo.png";
    }
}
