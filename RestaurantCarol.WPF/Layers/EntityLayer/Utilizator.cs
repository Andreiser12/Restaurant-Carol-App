using RestaurantCarol.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Layers
{
    public class Utilizator
    {
        public int IdUtilizator { get; set; }
        public string Nume { get; set; } = string.Empty;
        public string Prenume { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? AdresaLivrare { get; set; }
        public string ParolaHash { get; set; } = string.Empty;
        public RolUtilizator RolUtilizator { get; set; } = RolUtilizator.Client;
    }
}


