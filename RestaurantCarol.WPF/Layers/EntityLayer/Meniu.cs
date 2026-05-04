using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Layers
{
    public class Meniu
    {
        public int IdMeniu { get; set; }
        public string Denumire { get; set; } = string.Empty;

        public int IdCategorie { get; set; }
        public Categorie? Categorie { get; set; }

        public List<MeniuPreparatItem> Componente { get; set; } = new();
    }
}

