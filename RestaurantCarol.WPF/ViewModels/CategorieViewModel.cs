using RestaurantCarol.Commands;
using RestaurantCarol.Layers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #region Commands

        private ICommand? addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand ??= new RelayCommand<Categorie>(categorieBLL.AddCategorie);
                return addCommand;
            }
        }

        private ICommand? modifyCommand;
        public ICommand ModifyCommand
        {
            get
            {
                modifyCommand ??= new RelayCommand<Categorie>(categorieBLL.ModifyCategorie);
                return modifyCommand;
            }
        }

        private ICommand? deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand ??= new RelayCommand<Categorie>(categorieBLL.DeleteCategorie);
                return deleteCommand;
            }
        }

        #endregion
    }
}

