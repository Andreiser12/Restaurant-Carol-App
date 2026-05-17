using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantCarol.Exceptions;
using RestaurantCarol.Layers;
namespace RestaurantCarol.Views
{
    public partial class AdreseView : Window
    {
        private AdresaBLL adresaBLL = new AdresaBLL();
        private int idUtilizator;
        public event Action? AdreseModificate;
        public AdreseView()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }
        public AdreseView(int idUtilizator) : this()
        {
            this.idUtilizator = idUtilizator;
            IncarcaAdrese();
        }
        private void IncarcaAdrese()
        {
            try
            {
                ObservableCollection<Adresa> adrese = adresaBLL.GetByUtilizator(idUtilizator);
                adreseList.DataContext = adrese;
                if (adrese.Count == 0)
                {
                    mesajGolText.Visibility = Visibility.Visible;
                    adreseList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mesajGolText.Visibility = Visibility.Collapsed;
                    adreseList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la incarcare adrese: {ex.Message}",
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Adauga_Click(object sender, RoutedEventArgs e)
        {
            AddEditAdresaView popup = new AddEditAdresaView(idUtilizator);
            popup.Owner = this;
            popup.ShowDialog();
            if (popup.AdresaRezultat != null)
            {
                try
                {
                    Adresa adresaNoua = popup.AdresaRezultat;
                    var adreseExistente = adresaBLL.GetByUtilizator(idUtilizator);
                    if (adreseExistente.Count == 0)
                    {
                        adresaNoua.EsteImplicita = true;
                    }
                    adresaBLL.AddAdresa(adresaNoua);
                    IncarcaAdrese();
                    AdreseModificate?.Invoke();
                }
                catch (RestaurantException ex)
                {
                    MessageBox.Show(ex.Message, "Eroare validare",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la adaugare: {ex.Message}",
                        "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Adresa adresa)
            {
                Adresa copie = new Adresa
                {
                    IdAdresa = adresa.IdAdresa,
                    IdUtilizator = adresa.IdUtilizator,
                    AdresaText = adresa.AdresaText,
                    EsteImplicita = adresa.EsteImplicita
                };
                AddEditAdresaView popup = new AddEditAdresaView(copie);
                popup.Owner = this;
                popup.ShowDialog();
                if (popup.AdresaRezultat != null)
                {
                    try
                    {
                        adresaBLL.ModifyAdresa(popup.AdresaRezultat);
                        IncarcaAdrese();
                        AdreseModificate?.Invoke();
                    }
                    catch (RestaurantException ex)
                    {
                        MessageBox.Show(ex.Message, "Eroare validare",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Eroare la modificare: {ex.Message}",
                            "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void SetImplicita_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Adresa adresa)
            {
                try
                {
                    adresaBLL.SetImplicita(adresa.IdAdresa);
                    IncarcaAdrese();
                    AdreseModificate?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la setare implicita: {ex.Message}",
                        "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Sterge_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Adresa adresa)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Sigur vrei sa stergi aceasta adresa?\n\n{adresa.AdresaText}",
                    "Confirmare stergere",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                try
                {
                    adresaBLL.DeleteAdresa(adresa.IdAdresa);
                    IncarcaAdrese();
                    AdreseModificate?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la stergere: {ex.Message}",
                        "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}