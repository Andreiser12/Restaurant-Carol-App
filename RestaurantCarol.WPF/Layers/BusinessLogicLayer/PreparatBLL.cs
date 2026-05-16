using RestaurantCarol.Exceptions;
using System.Collections.ObjectModel;

namespace RestaurantCarol.Layers
{
    public class PreparatBLL
    {
        private PreparatDAL preparatDAL = new PreparatDAL();

        public ObservableCollection<Preparat> GetByCategorie(int idCategorie)
        {
            return preparatDAL.GetByCategorie(idCategorie);
        }

        public ObservableCollection<Alergen> GetAlergeniByPreparat(int idPreparat)
        {
            return preparatDAL.GetAlergeniByPreparat(idPreparat);
        }

        public ObservableCollection<Preparat> GetAllPreparate()
        {
            return preparatDAL.GetAllPreparate();
        }

        public ObservableCollection<Preparat> GetTopPopulare(int top =3)
        {
            return preparatDAL.GetTopPopulare(top);
        }

        public ObservableCollection<Alergen> GetAllAlergeni()
        {
            return preparatDAL.GetAllAlergeni();
        }

        public void AddPreparat(Preparat preparat, List<int> idsAlergeni, string? caleFotografie)
        {
            if (preparat == null)
                throw new RestaurantException("Preparat invalid.");

            if (string.IsNullOrWhiteSpace(preparat.Denumire))
                throw new RestaurantException("Denumirea preparatului este obligatorie.");

            if (preparat.Denumire.Length > 200)
                throw new RestaurantException("Denumirea preparatului poate avea maxim 200 caractere.");

            if (preparat.Pret <= 0)
                throw new RestaurantException("Pretul trebuie sa fie mai mare decat 0.");

            if (preparat.CantitatePortie <= 0)
                throw new RestaurantException("Cantitatea per portie trebuie sa fie mai mare decat 0.");

            if (preparat.CantitateTotala < 0)
                throw new RestaurantException("Cantitatea totala nu poate fi negativa.");

            if (preparat.IdCategorie <= 0)
                throw new RestaurantException("Selectati o categorie pentru preparat.");

            if (preparat.Calorii.HasValue && preparat.Calorii.Value < 0)
                throw new RestaurantException("Caloriile nu pot fi negative.");

            if (preparat.Grasimi.HasValue && preparat.Grasimi.Value < 0)
                throw new RestaurantException("Grasimile nu pot fi negative.");

            if (preparat.Carbohidrati.HasValue && preparat.Carbohidrati.Value < 0)
                throw new RestaurantException("Carbohidratii nu pot fi negativi.");

            if (preparat.Proteine.HasValue && preparat.Proteine.Value < 0)
                throw new RestaurantException("Proteinele nu pot fi negative.");

            if (preparat.Sare.HasValue && preparat.Sare.Value < 0)
                throw new RestaurantException("Sarea nu poate fi negativa.");

            preparat.Denumire = preparat.Denumire.Trim();
            if (!string.IsNullOrWhiteSpace(preparat.Descriere))
                preparat.Descriere = preparat.Descriere.Trim();

            try
            {
                preparatDAL.AddPreparat(preparat, idsAlergeni, caleFotografie);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new RestaurantException("Exista deja un preparat cu acest nume.");
            }
        }
    }
}