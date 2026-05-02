using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Models
{
    public class ItemComanda
    {
        public int IdItemComanda { get; set; }
        public int IdComanda { get; set; }

        public int? IdPreparat { get; set; }
        public int? IdMeniu { get; set; }

        public int Cantitate { get; set; }

        public Preparat? Preparat { get; set; }
        public Meniu? Meniu { get; set; }
    }
}
