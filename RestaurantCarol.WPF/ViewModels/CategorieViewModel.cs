using RestaurantCarol.Layers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.ViewModels
{
    public class CategorieViewModel
    {
        private CategorieBLL categorieBLL = new CategorieBLL();

        public CategorieViewModel()
        {
            CategoriiList = categorieBLL.GetAllCategorii();
        }

        public ObservableCollection<Categorie>? CategoriiList
        {
            get => categorieBLL.CategoriiList;
            set => categorieBLL.CategoriiList = value;
        }
    }
}
