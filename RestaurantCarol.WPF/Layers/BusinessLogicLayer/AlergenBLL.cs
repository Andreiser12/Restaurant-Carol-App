using System.Collections.ObjectModel;

namespace RestaurantCarol.Layers
{
    public class AlergenBLL
    {
        private AlergenDAL alergenDAL = new AlergenDAL();

        public ObservableCollection<Alergen> GetAllAlergeni()
        {
            return alergenDAL.GetAllAlergeni();
        }
    }
}