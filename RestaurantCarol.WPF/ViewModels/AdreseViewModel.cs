using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public class AdreseViewModel : ViewModelBase
    {
        public event Action? AdreseModificate;

        public event Action? InchideRequested;

        public event Func<Adresa?>? DeschideAddPopupCerere;

        public event Func<Adresa, Adresa?>? DeschideEditPopupCerere;

        private AdresaBLL adresaBLL = new AdresaBLL();
        private int idUtilizator;

        public ObservableCollection<Adresa> Adrese { get; } = new();

        public Visibility VizibilitateMesajGol =>
            Adrese.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VizibilitateLista =>
            Adrese.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public AdreseViewModel(int idUtilizator)
        {
            this.idUtilizator = idUtilizator;
            IncarcaAdrese();
        }

        private void IncarcaAdrese()
        {
            try
            {
                Adrese.Clear();
                var dinDb = adresaBLL.GetByUtilizator(idUtilizator);
                foreach (var a in dinDb)
                    Adrese.Add(a);

                NotifyPropertyChanged(nameof(VizibilitateMesajGol));
                NotifyPropertyChanged(nameof(VizibilitateLista));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare adrese: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand AdaugaCommand => new RelayCommand<object>(_ => Adauga());
        public ICommand EditCommand => new RelayCommand<Adresa>(EditAdresa);
        public ICommand SetImplicitaCommand => new RelayCommand<Adresa>(SetImplicita);
        public ICommand StergeCommand => new RelayCommand<Adresa>(StergeAdresa);
        public ICommand InchideCommand => new RelayCommand<object>(_ => InchideRequested?.Invoke());

        private void Adauga()
        {
            Adresa? adresaNoua = DeschideAddPopupCerere?.Invoke();
            if (adresaNoua == null) return;

            if (Adrese.Count == 0)
                adresaNoua.EsteImplicita = true;

            adresaBLL.AddAdresa(adresaNoua);
            IncarcaAdrese();
            AdreseModificate?.Invoke();
        }

        private void EditAdresa(Adresa adresa)
        {
            Adresa copie = new Adresa
            {
                IdAdresa = adresa.IdAdresa,
                IdUtilizator = adresa.IdUtilizator,
                AdresaText = adresa.AdresaText,
                EsteImplicita = adresa.EsteImplicita
            };

            Adresa? adresaModificata = DeschideEditPopupCerere?.Invoke(copie);
            if (adresaModificata == null) return;

            adresaBLL.ModifyAdresa(adresaModificata);
            IncarcaAdrese();
            AdreseModificate?.Invoke();
        }

        private void SetImplicita(Adresa adresa)
        {
            adresaBLL.SetImplicita(adresa.IdAdresa);
            IncarcaAdrese();
            AdreseModificate?.Invoke();
        }

        private void StergeAdresa(Adresa adresa)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Sigur vrei sa stergi aceasta adresa?\n\n{adresa.AdresaText}",
                "Confirmare stergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            adresaBLL.DeleteAdresa(adresa.IdAdresa);
            IncarcaAdrese();
            AdreseModificate?.Invoke();
        }
    }
}