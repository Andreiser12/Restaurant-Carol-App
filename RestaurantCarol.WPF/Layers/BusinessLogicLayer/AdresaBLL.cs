using System.Collections.ObjectModel;
using RestaurantCarol.Exceptions;

namespace RestaurantCarol.Layers
{
    public class AdresaBLL
    {
        private AdresaDAL adresaDAL = new AdresaDAL();

        public ObservableCollection<Adresa> GetByUtilizator(int idUtilizator)
        {
            return adresaDAL.GetByUtilizator(idUtilizator);
        }

        public Adresa? GetAdresaImplicita(int idUtilizator)
        {
            return adresaDAL.GetAdresaImplicita(idUtilizator);
        }

        public void AddAdresa(Adresa adresa)
        {
            ValidateAdresa(adresa);
            adresaDAL.AddAdresa(adresa);
        }

        public void ModifyAdresa(Adresa adresa)
        {
            if (adresa.IdAdresa <= 0)
                throw new RestaurantException("Trebuie selectata o adresa valida.");

            ValidateAdresa(adresa);
            adresaDAL.ModifyAdresa(adresa);
        }

        public void SetImplicita(int idAdresa)
        {
            if (idAdresa <= 0)
                throw new RestaurantException("Trebuie selectata o adresa valida.");

            adresaDAL.SetImplicita(idAdresa);
        }

        public void DeleteAdresa(int idAdresa)
        {
            if (idAdresa <= 0)
                throw new RestaurantException("Trebuie selectata o adresa valida.");

            adresaDAL.DeleteAdresa(idAdresa);
        }

        private void ValidateAdresa(Adresa adresa)
        {
            if (adresa == null)
                throw new RestaurantException("Adresa nu poate fi nula.");

            if (string.IsNullOrWhiteSpace(adresa.AdresaText))
                throw new RestaurantException("Textul adresei este obligatoriu.");

            if (adresa.AdresaText.Trim().Length < 5)
                throw new RestaurantException("Adresa trebuie sa aiba minim 5 caractere.");

            if (adresa.IdUtilizator <= 0)
                throw new RestaurantException("Trebuie precizat utilizatorul pentru adresa.");
        }
    }
}