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

        public Preparat? GetById(int idPreparat)
        {
            return preparatDAL.GetById(idPreparat);
        }

        public bool CheckDenumireDuplicate(string denumire, int idExclude)
        {
            if (string.IsNullOrWhiteSpace(denumire)) return false;
            return preparatDAL.CheckDenumireDuplicate(denumire.Trim(), idExclude);
        }

        public void UpdatePreparat(Preparat preparat, List<int> idsAlergeni,
                                    string actiunePoza, string? caleFotografieNoua)
        {
            if (preparat == null)
                throw new RestaurantException("Preparat invalid.");

            if (preparat.IdPreparat <= 0)
                throw new RestaurantException("Preparat fara id valid.");

            if (string.IsNullOrWhiteSpace(preparat.Denumire))
                throw new RestaurantException("Denumirea preparatului este obligatorie.");

            if (preparat.Denumire.Length > 200)
                throw new RestaurantException("Denumirea preparatului poate avea maxim 200 caractere.");

            if (preparat.Pret <= 0)
                throw new RestaurantException("Pretul trebuie sa fie mai mare decat 0.");

            if (preparat.CantitatePortie <= 0)
                throw new RestaurantException("Cantitatea per portie trebuie sa fie mai mare decat 0.");

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

            if (CheckDenumireDuplicate(preparat.Denumire, preparat.IdPreparat))
                throw new RestaurantException($"Exista deja un alt preparat cu denumirea '{preparat.Denumire}'.");

            if (actiunePoza != "pastreaza" && actiunePoza != "sterge" && actiunePoza != "inlocuieste")
                throw new RestaurantException("Actiune poza invalida.");

            preparatDAL.UpdatePreparat(preparat, idsAlergeni, actiunePoza, caleFotografieNoua);
        }

        public void UpdateStoc(int idPreparat, int cantitateNoua)
        {
            if (idPreparat <= 0)
                throw new RestaurantException("Id preparat invalid.");

            if (cantitateNoua < 0)
                throw new RestaurantException("Cantitatea nu poate fi negativa.");

            preparatDAL.UpdateStoc(idPreparat, cantitateNoua);
        }

        public ObservableCollection<Preparat> GetPreparateStocRedus(int prag)
        {
            if (prag < 0) return new ObservableCollection<Preparat>();
            return preparatDAL.GetPreparateStocRedus(prag);
        }

        public void DeletePreparat(int idPreparat)
        {
            if (idPreparat <= 0)
                throw new RestaurantException("Id preparat invalid.");

            try
            {
                string? calePoza = preparatDAL.DeletePreparat(idPreparat);

                if (!string.IsNullOrEmpty(calePoza))
                {
                    StergePozaDeOnDisk(calePoza);
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 52001)
            {
                throw new RestaurantException("Preparatul nu mai exista.");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 52002)
            {
                throw new RestaurantException(
                    "Acest preparat nu poate fi sters pentru ca este folosit in comenzi existente.");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 52003)
            {
                throw new RestaurantException(
                    "Acest preparat face parte din meniuri compuse si nu poate fi sters.");
            }
        }

        public void StergePozaDeOnDisk(string calePoza)
        {
            try
            {
                if (!calePoza.Contains("runtime/", StringComparison.OrdinalIgnoreCase) &&
                    !calePoza.Contains("runtime\\", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                string caleAbsoluta = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    calePoza.Replace("/", "\\"));

                if (System.IO.File.Exists(caleAbsoluta))
                {
                    System.IO.File.Delete(caleAbsoluta);
                }
            }
            catch {}
        }
    }
}