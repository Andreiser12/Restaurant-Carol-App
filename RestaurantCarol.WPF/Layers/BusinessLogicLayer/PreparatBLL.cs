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
    }
}