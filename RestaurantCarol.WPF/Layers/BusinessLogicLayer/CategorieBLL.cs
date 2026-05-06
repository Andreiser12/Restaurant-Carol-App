using RestaurantCarol.Exceptions;
using System.Collections.ObjectModel;

namespace RestaurantCarol.Layers
{
    public class CategorieBLL
    {
        public ObservableCollection<Categorie>? CategoriiList { get; set; }

        private CategorieDAL categorieDAL = new CategorieDAL();

        public ObservableCollection<Categorie> GetAllCategorii()
        {
            CategoriiList = categorieDAL.GetAllCategorii();
            return CategoriiList;
        }

        public ObservableCollection<Categorie> GetCategoriiByTip(TipCategorie tip)
        {
            return categorieDAL.GetCategoriiByTip(tip);
        }

        public void AddCategorie(Categorie categorie)
        {
            if (categorie == null)
            {
                throw new RestaurantException("Trebuie sa precizati o categorie.");
            }
            if (string.IsNullOrWhiteSpace(categorie.Denumire))
            {
                throw new RestaurantException("Denumirea categoriei trebuie precizata.");
            }

            categorieDAL.AddCategorie(categorie);
            CategoriiList?.Add(categorie);
        }

        public void ModifyCategorie(Categorie categorie)
        {
            if (categorie == null)
            {
                throw new RestaurantException("Trebuie selectata o categorie.");
            }
            if (string.IsNullOrWhiteSpace(categorie.Denumire))
            {
                throw new RestaurantException("Denumirea categoriei trebuie precizata.");
            }

            categorieDAL.ModifyCategorie(categorie);
        }

        public void DeleteCategorie(Categorie categorie)
        {
            if (categorie == null)
            {
                throw new RestaurantException("Trebuie selectata o categorie.");
            }

            categorieDAL.DeleteCategorie(categorie);
            CategoriiList?.Remove(categorie);
        }
    }
}
