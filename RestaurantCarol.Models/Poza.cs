using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Models
{
    public class Poza
    {
        public int IdFotografie { get; set; }
        public int IdPreparat { get; set; }
        public string CaleFisier { get; set; } = string.Empty;
    }
}
