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
    }
}