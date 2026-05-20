using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AdaugaMeniuViewModel : ViewModelBase
    {
        public event Action<bool>? InchideRequested;

        private MeniuBLL meniuBLL = new MeniuBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();
        private PreparatBLL preparatBLL = new PreparatBLL();

        public ObservableCollection<Categorie> Categorii { get; } = new();
        public ObservableCollection<PreparatSelectabil> Preparate { get; } = new();

        private string denumire = string.Empty;
        public string Denumire
        {
            get => denumire;
            set { denumire = value; NotifyPropertyChanged(); }
        }

        private Categorie? categorieSelectata;
        public Categorie? CategorieSelectata
        {
            get => categorieSelectata;
            set
            {
                categorieSelectata = value;
                NotifyPropertyChanged();
                IncarcaPreparatePentruCategorie();
            }
        }

        public AdaugaMeniuViewModel()
        {
            IncarcaCategorii();
        }

        private void IncarcaCategorii()
        {
            try
            {
                var categorii = categorieBLL.GetAllCategorii();
                Categorii.Clear();
                foreach (var c in categorii)
                    Categorii.Add(c);

                CategorieSelectata = Categorii.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea categoriilor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IncarcaPreparatePentruCategorie()
        {
            try
            {
                Preparate.Clear();

                if (CategorieSelectata == null) return;

                var preparate = preparatBLL.GetAllPreparate()
                    .Where(p => p.IdCategorie == CategorieSelectata.IdCategorie);

                foreach (var p in preparate)
                    Preparate.Add(new PreparatSelectabil(p));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcarea preparatelor: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SalveazaCommand => new RelayCommand<object>(_ => Salveaza());
        public ICommand AnuleazaCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke(false));

        private void Salveaza()
        {
            if (CategorieSelectata == null)
                throw new RestaurantException("Selecteaza o categorie.");

            var componente = Preparate
                .Where(ps => ps.IsBifat)
                .Select(ps => new MeniuPreparatItem
                {
                    IdPreparat = ps.IdPreparat,
                    CantitatePortie = ps.CantitatePortie,
                    Preparat = ps.Preparat
                })
                .ToList();

            meniuBLL.AddMeniu(Denumire.Trim(), CategorieSelectata.IdCategorie, componente);

            MessageBox.Show("Meniul a fost adaugat.", "Succes",
                MessageBoxButton.OK, MessageBoxImage.Information);

            InchideRequested?.Invoke(true);
        }
    }
}