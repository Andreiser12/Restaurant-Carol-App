using System.Collections.ObjectModel;
namespace RestaurantCarol.Layers
{
    public class CatalogBLL
    {
        private PreparatBLL preparatBLL = new PreparatBLL();
        private MeniuBLL meniuBLL = new MeniuBLL();

        public ObservableCollection<CatalogItem> GetByCategorie(int idCategorie)
        {
            ObservableCollection<CatalogItem> items = new();
            foreach (var preparat in preparatBLL.GetByCategorie(idCategorie))
                items.Add(CatalogItem.DinPreparat(preparat));
            foreach (var meniu in meniuBLL.GetByCategorie(idCategorie))
                items.Add(CatalogItem.DinMeniu(meniu));
            return items;
        }
    }
}
