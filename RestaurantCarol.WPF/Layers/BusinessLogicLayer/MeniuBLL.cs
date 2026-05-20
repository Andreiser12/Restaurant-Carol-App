using System.Collections.ObjectModel;
using RestaurantCarol.Exceptions;
namespace RestaurantCarol.Layers
{
    public class MeniuBLL
    {
        private MeniuDAL meniuDAL = new MeniuDAL();
        public ObservableCollection<Meniu> GetByCategorie(int idCategorie)
        {
            var meniuri = meniuDAL.GetByCategorie(idCategorie);
            foreach (var meniu in meniuri)
                CalculeazaProprietati(meniu);
            return meniuri;
        }

        public void CalculeazaProprietati(Meniu meniu)
        {
            decimal suma = 0;
            bool disponibil = meniu.Componente.Count > 0;
            foreach (var c in meniu.Componente)
            {
                suma += c.PretPreparat;
                if (c.Preparat == null || c.Preparat.CantitateTotala < c.CantitatePortie)
                    disponibil = false;
            }
            decimal reducere = AppSettingsHelper.DiscountMeniu;
            meniu.Pret = Math.Round(suma * (100 - reducere) / 100, 2);
            meniu.EsteDisponibil = disponibil;
        }

        public int AddMeniu(string denumire, int idCategorie, List<MeniuPreparatItem> componente)
        {
            if (string.IsNullOrWhiteSpace(denumire))
                throw new RestaurantException("Denumirea meniului este obligatorie.");
            if (idCategorie <= 0)
                throw new RestaurantException("Selecteaza o categorie.");
            if (componente == null || componente.Count == 0)
                throw new RestaurantException("Meniul trebuie sa contina cel putin un preparat.");
            return meniuDAL.AddMeniu(denumire.Trim(), idCategorie, componente);
        }
    }
}
