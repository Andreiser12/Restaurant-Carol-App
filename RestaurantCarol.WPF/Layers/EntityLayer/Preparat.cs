using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Models
{
    public class Preparat
    {
        public int IdPreparat { get; set; }
        public string Denumire { get; set; } = string.Empty;
        public decimal Pret { get; set; }
        public decimal CantitatePortie { get; set; }
        public decimal CantitateTotala { get; set; }
        public string? Descriere { get; set; }

        public int? Calorii { get; set; }
        public decimal? Grasimi { get; set; }
        public decimal? Carbohidrati { get; set; }
        public decimal? Proteine { get; set; }
        public decimal? Sare { get; set; }

        public int IdCategorie { get; set; }
        public Categorie? Categorie { get; set; }

        public List<Alergen> Alergeni { get; set; } = new();
        public List<Poza> Poze { get; set; } = new();

    }
}
