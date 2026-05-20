using RestaurantCarol.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RestaurantCarol.Layers
{
    public class Comanda
    {
        public int IdComanda { get; set; }

        public string CodComanda { get; set; } = string.Empty;

        public int IdUtilizator { get; set; }

        public DateTime DataComanda { get; set; }

        public DateTime? OraEstimataLivrare { get; set; }

        public StareComanda StareComanda { get; set; } = StareComanda.Inregistrata;

        public decimal CostMancare { get; set; }

        public decimal CostTransport { get; set; }

        public decimal Discount { get; set; }

        public decimal CostTotal => CostMancare + CostTransport - Discount;

        public Utilizator? Utilizator { get; set; }

        public string AdresaLivrareCompleta { get; set; } = string.Empty;

        public List<ItemComanda> Items { get; set; } = new();

        public string StareAfisata => StareComandaHelper.GetDenumireAfisata(StareComanda);
    }
}
