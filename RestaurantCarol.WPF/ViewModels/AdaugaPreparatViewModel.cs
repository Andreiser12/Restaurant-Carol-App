using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RestaurantCarol.Commands;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Helpers;
using RestaurantCarol.Layers;

namespace RestaurantCarol.ViewModels
{
    public enum ModFormularPreparat
    {
        Add,
        Edit
    }

    public class AdaugaPreparatViewModel : ViewModelBase
    {
        public event Action? PreparatModificat;

        public event Action? InchideRequested;

        public event Action<string>? AfiseazaPreviewPozaCerere;

        public event Action? AscundePreviewPozaCerere;

        private PreparatBLL preparatBLL = new PreparatBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();

        private readonly ModFormularPreparat mod;
        private Preparat? preparatCurent;
        private string? calePozaSursa;
        private string? calePozaVeche;
        private string actiunePoza = "pastreaza";

        private ObservableCollection<Categorie> categoriiMancare = new();
        private ObservableCollection<Categorie> categoriiBautura = new();

        public ObservableCollection<Categorie> CategoriiAfisate { get; } = new();
        public ObservableCollection<AlergenSelectabil> Alergeni { get; } = new();

        private bool tabPreparatActiv = true;
        public bool TabPreparatActiv
        {
            get => tabPreparatActiv;
            set
            {
                tabPreparatActiv = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TabCategorieActiv));
                NotifyPropertyChanged(nameof(VizibilitateFormPreparat));
                NotifyPropertyChanged(nameof(VizibilitateFormCategorie));
                NotifyPropertyChanged(nameof(TextSubmit));
            }
        }

        public bool TabCategorieActiv => !tabPreparatActiv;

        public Visibility VizibilitateFormPreparat =>
            tabPreparatActiv ? Visibility.Visible : Visibility.Collapsed;

        public Visibility VizibilitateFormCategorie =>
            tabPreparatActiv ? Visibility.Collapsed : Visibility.Visible;

        public string TitluTabPreparat => mod == ModFormularPreparat.Edit
            ? "Modifica preparat"
            : "Adauga preparat";

        public Visibility VizibilitateTabCategorie => mod == ModFormularPreparat.Edit
            ? Visibility.Collapsed
            : Visibility.Visible;

        public Visibility VizibilitateButonSterge => mod == ModFormularPreparat.Edit
            ? Visibility.Visible
            : Visibility.Collapsed;

        public string TextSubmit
        {
            get
            {
                if (TabCategorieActiv) return "Adauga categorie";
                return mod == ModFormularPreparat.Edit
                    ? "Salveaza modificarile"
                    : "Adauga preparat";
            }
        }

        private bool tipMancare = true;
        public bool TipMancare
        {
            get => tipMancare;
            set
            {
                if (tipMancare == value) return;
                tipMancare = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TipBautura));
                ActualizeazaCategoriiAfisate();
            }
        }

        public bool TipBautura
        {
            get => !tipMancare;
            set => TipMancare = !value;
        }

        private Categorie? categorieSelectata;
        public Categorie? CategorieSelectata
        {
            get => categorieSelectata;
            set { categorieSelectata = value; NotifyPropertyChanged(); }
        }

        private string denumire = string.Empty;
        public string Denumire
        {
            get => denumire;
            set { denumire = value; NotifyPropertyChanged(); }
        }

        private string pret = string.Empty;
        public string Pret
        {
            get => pret;
            set { pret = value; NotifyPropertyChanged(); }
        }

        private string cantitatePortie = string.Empty;
        public string CantitatePortie
        {
            get => cantitatePortie;
            set { cantitatePortie = value; NotifyPropertyChanged(); }
        }

        private string descriere = string.Empty;
        public string Descriere
        {
            get => descriere;
            set { descriere = value; NotifyPropertyChanged(); }
        }

        private string calorii = string.Empty;
        public string Calorii
        {
            get => calorii;
            set { calorii = value; NotifyPropertyChanged(); }
        }

        private string grasimi = string.Empty;
        public string Grasimi
        {
            get => grasimi;
            set { grasimi = value; NotifyPropertyChanged(); }
        }

        private string carbohidrati = string.Empty;
        public string Carbohidrati
        {
            get => carbohidrati;
            set { carbohidrati = value; NotifyPropertyChanged(); }
        }

        private string proteine = string.Empty;
        public string Proteine
        {
            get => proteine;
            set { proteine = value; NotifyPropertyChanged(); }
        }

        private string sare = string.Empty;
        public string Sare
        {
            get => sare;
            set { sare = value; NotifyPropertyChanged(); }
        }

        private string textPoza = "Nicio poza selectata";
        public string TextPoza
        {
            get => textPoza;
            set { textPoza = value; NotifyPropertyChanged(); }
        }

        private Visibility vizibilitateButonStergePoza = Visibility.Collapsed;
        public Visibility VizibilitateButonStergePoza
        {
            get => vizibilitateButonStergePoza;
            set { vizibilitateButonStergePoza = value; NotifyPropertyChanged(); }
        }

        private bool tipCatMancare = true;
        public bool TipCatMancare
        {
            get => tipCatMancare;
            set
            {
                tipCatMancare = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TipCatBautura));
            }
        }

        public bool TipCatBautura
        {
            get => !tipCatMancare;
            set => TipCatMancare = !value;
        }

        private string catDenumire = string.Empty;
        public string CatDenumire
        {
            get => catDenumire;
            set { catDenumire = value; NotifyPropertyChanged(); }
        }

        public AdaugaPreparatViewModel()
        {
            mod = ModFormularPreparat.Add;
            IncarcaDateInitiale();
        }

        public AdaugaPreparatViewModel(int idPreparat)
        {
            mod = ModFormularPreparat.Edit;
            IncarcaDateInitiale();

            preparatCurent = preparatBLL.GetById(idPreparat);
            if (preparatCurent == null)
            {
                MessageBox.Show("Preparatul nu mai exista in baza de date.",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                InchideRequested?.Invoke();
                return;
            }

            calePozaVeche = preparatCurent.PrimaCalePoza;
            PopuleazaFormularulCuPreparat();
        }

        private void IncarcaDateInitiale()
        {
            try
            {
                var alergeniDB = preparatBLL.GetAllAlergeni();
                foreach (var a in alergeniDB)
                    Alergeni.Add(new AlergenSelectabil(a, isBifat: false));

                categoriiMancare = categorieBLL.GetCategoriiByTip(TipCategorie.Mancare);
                categoriiBautura = categorieBLL.GetCategoriiByTip(TipCategorie.Bauturi);

                ActualizeazaCategoriiAfisate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare date: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizeazaCategoriiAfisate()
        {
            CategoriiAfisate.Clear();
            var sursa = tipMancare ? categoriiMancare : categoriiBautura;
            foreach (var c in sursa)
                CategoriiAfisate.Add(c);

            CategorieSelectata = CategoriiAfisate.FirstOrDefault();
        }

        private void PopuleazaFormularulCuPreparat()
        {
            if (preparatCurent == null) return;

            var categoriaPreparat = categoriiMancare
                .Concat(categoriiBautura)
                .FirstOrDefault(c => c.IdCategorie == preparatCurent.IdCategorie);

            if (categoriaPreparat != null)
            {
                TipMancare = categoriaPreparat.Tip == TipCategorie.Mancare;

                CategorieSelectata = CategoriiAfisate
                    .FirstOrDefault(c => c.IdCategorie == preparatCurent.IdCategorie);
            }

            Denumire = preparatCurent.Denumire;
            Pret = preparatCurent.Pret.ToString("F2", CultureInfo.InvariantCulture);
            CantitatePortie = preparatCurent.CantitatePortie.ToString();
            Descriere = preparatCurent.Descriere ?? "";

            Calorii = preparatCurent.Calorii?.ToString() ?? "";
            Grasimi = preparatCurent.Grasimi?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
            Carbohidrati = preparatCurent.Carbohidrati?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
            Proteine = preparatCurent.Proteine?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
            Sare = preparatCurent.Sare?.ToString("F2", CultureInfo.InvariantCulture) ?? "";

            var idsAlergeniCurenti = preparatBLL.GetAlergeniByPreparat(preparatCurent.IdPreparat)
                .Select(a => a.IdAlergen).ToHashSet();

            foreach (var a in Alergeni)
                a.IsBifat = idsAlergeniCurenti.Contains(a.IdAlergen);

            if (!string.IsNullOrEmpty(preparatCurent.PrimaCalePoza))
            {
                TextPoza = Path.GetFileName(preparatCurent.PrimaCalePoza);
                VizibilitateButonStergePoza = Visibility.Visible;
                AfiseazaPreviewPozaCerere?.Invoke(preparatCurent.PrimaCalePoza);
            }
        }

        public ICommand TabPreparatCommand =>
            new RelayCommand<object>(_ => TabPreparatActiv = true);

        public ICommand TabCategorieCommand =>
            new RelayCommand<object>(_ => TabPreparatActiv = false);

        public ICommand AlegePozaCommand =>
            new RelayCommand<object>(_ => AlegePoza());

        public ICommand StergePozaCommand =>
            new RelayCommand<object>(_ => StergePoza());

        public ICommand SubmitCommand =>
            new RelayCommand<object>(_ => Submit());

        public ICommand StergePreparatCommand =>
            new RelayCommand<object>(_ => StergePreparat());

        public ICommand InchideCommand =>
            new RelayCommand<object>(_ => InchideRequested?.Invoke());


        private void AlegePoza()
        {
            string? cale = ImageUploadHelper.AlegePoza();
            if (cale == null) return;

            try
            {
                calePozaSursa = cale;
                TextPoza = Path.GetFileName(cale);
                VizibilitateButonStergePoza = Visibility.Visible;

                AfiseazaPreviewPozaCerere?.Invoke(cale);

                if (mod == ModFormularPreparat.Edit)
                    actiunePoza = "inlocuieste";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nu pot incarca poza: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                calePozaSursa = null;
                TextPoza = "Nicio poza selectata";
                VizibilitateButonStergePoza = Visibility.Collapsed;
                AscundePreviewPozaCerere?.Invoke();
            }
        }

        private void StergePoza()
        {
            calePozaSursa = null;
            TextPoza = "Nicio poza selectata";
            VizibilitateButonStergePoza = Visibility.Collapsed;
            AscundePreviewPozaCerere?.Invoke();

            if (mod == ModFormularPreparat.Edit)
                actiunePoza = "sterge";
        }

        private void Submit()
        {
            if (TabPreparatActiv)
            {
                if (mod == ModFormularPreparat.Add)
                    SubmitPreparatAdd();
                else
                    SubmitPreparatEdit();
            }
            else
            {
                SubmitCategorie();
            }
        }

        private void SubmitPreparatAdd()
        {
            if (CategorieSelectata == null)
                throw new RestaurantException("Selecteaza o categorie.");

            Preparat preparat = ConstruiestePreparatDinFormular(CategorieSelectata.IdCategorie);
            List<int> idsAlergeni = Alergeni.Where(a => a.IsBifat).Select(a => a.IdAlergen).ToList();

            if (preparatBLL.CheckDenumireDuplicate(preparat.Denumire, 0))
                throw new RestaurantException($"Exista deja un preparat cu denumirea '{preparat.Denumire}'.");

            string? caleFotografieRelativa = null;
            if (!string.IsNullOrEmpty(calePozaSursa))
            {
                string prefix = SanitizeazaNume(preparat.Denumire);
                caleFotografieRelativa = ImageUploadHelper.CopiazaPozaInRuntime(calePozaSursa, prefix);
            }

            preparatBLL.AddPreparat(preparat, idsAlergeni, caleFotografieRelativa);

            MessageBox.Show($"Preparatul '{preparat.Denumire}' a fost adaugat cu succes!",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            PreparatModificat?.Invoke();
            InchideRequested?.Invoke();
        }

        private void SubmitPreparatEdit()
        {
            if (preparatCurent == null)
                throw new RestaurantException("Preparat invalid.");

            if (CategorieSelectata == null)
                throw new RestaurantException("Selecteaza o categorie.");

            Preparat preparat = ConstruiestePreparatDinFormular(CategorieSelectata.IdCategorie);
            preparat.IdPreparat = preparatCurent.IdPreparat;

            List<int> idsAlergeni = Alergeni.Where(a => a.IsBifat).Select(a => a.IdAlergen).ToList();

            string? caleFotografieNoua = null;
            if (actiunePoza == "inlocuieste" && !string.IsNullOrEmpty(calePozaSursa))
            {
                string prefix = SanitizeazaNume(preparat.Denumire);
                caleFotografieNoua = ImageUploadHelper.CopiazaPozaInRuntime(calePozaSursa, prefix);
            }

            preparatBLL.UpdatePreparat(preparat, idsAlergeni, actiunePoza, caleFotografieNoua);

            if ((actiunePoza == "inlocuieste" || actiunePoza == "sterge")
                && !string.IsNullOrEmpty(calePozaVeche))
            {
                preparatBLL.StergePozaDeOnDisk(calePozaVeche);
            }

            MessageBox.Show($"Preparatul '{preparat.Denumire}' a fost actualizat cu succes!",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            PreparatModificat?.Invoke();
            InchideRequested?.Invoke();
        }

        private void SubmitCategorie()
        {
            Categorie cat = new Categorie
            {
                Denumire = CatDenumire,
                Tip = TipCatMancare ? TipCategorie.Mancare : TipCategorie.Bauturi
            };

            categorieBLL.AddCategorie(cat);

            MessageBox.Show($"Categoria '{cat.Denumire}' a fost adaugata cu succes!",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            PreparatModificat?.Invoke();
            InchideRequested?.Invoke();
        }

        private void StergePreparat()
        {
            if (preparatCurent == null) return;

            MessageBoxResult result = MessageBox.Show(
                $"Sigur vrei sa stergi preparatul '{preparatCurent.Denumire}'?\n\n" +
                "Atentie: stergerea NU este reversibila.\n" +
                "Daca preparatul a fost comandat anterior, nu poate fi sters " +
                "(pentru pastrarea istoricului).",
                "Confirmare stergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            preparatBLL.DeletePreparat(preparatCurent.IdPreparat);

            MessageBox.Show($"Preparatul '{preparatCurent.Denumire}' a fost sters cu succes!",
                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            PreparatModificat?.Invoke();
            InchideRequested?.Invoke();
        }

        private Preparat ConstruiestePreparatDinFormular(int idCategorie)
        {
            decimal pret = ParseDecimal(Pret, "Pret");
            int cantitatePortie = ParseInt(CantitatePortie, "Gramaj per portie");

            int? calorii = ParseOptionalInt(Calorii, "Calorii");
            decimal? grasimi = ParseOptionalDecimal(Grasimi, "Grasimi");
            decimal? carbohidrati = ParseOptionalDecimal(Carbohidrati, "Carbohidrati");
            decimal? proteine = ParseOptionalDecimal(Proteine, "Proteine");
            decimal? sare = ParseOptionalDecimal(Sare, "Sare");

            return new Preparat
            {
                Denumire = Denumire,
                Pret = pret,
                CantitatePortie = cantitatePortie,
                CantitateTotala = 0,
                Descriere = string.IsNullOrWhiteSpace(Descriere) ? null : Descriere,
                Calorii = calorii,
                Grasimi = grasimi,
                Carbohidrati = carbohidrati,
                Proteine = proteine,
                Sare = sare,
                IdCategorie = idCategorie
            };
        }

        private decimal ParseDecimal(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new RestaurantException($"Campul '{camp}' este obligatoriu.");

            text = text.Trim().Replace(",", ".");

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar valid.");

            return val;
        }

        private int ParseInt(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new RestaurantException($"Campul '{camp}' este obligatoriu.");

            if (!int.TryParse(text.Trim(), out int val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar intreg.");

            return val;
        }

        private decimal? ParseOptionalDecimal(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            text = text.Trim().Replace(",", ".");

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar valid sau gol.");

            return val;
        }

        private int? ParseOptionalInt(string text, string camp)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            if (!int.TryParse(text.Trim(), out int val))
                throw new RestaurantException($"'{camp}' trebuie sa fie un numar intreg sau gol.");

            return val;
        }

        private string SanitizeazaNume(string nume)
        {
            var caractereAcceptate = nume.ToLower()
                .Replace(" ", "_")
                .Where(c => char.IsLetterOrDigit(c) || c == '_')
                .ToArray();

            string rezultat = new string(caractereAcceptate);
            return string.IsNullOrEmpty(rezultat) ? "preparat" : rezultat;
        }
    }
}