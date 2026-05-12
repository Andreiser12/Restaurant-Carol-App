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
    }
}