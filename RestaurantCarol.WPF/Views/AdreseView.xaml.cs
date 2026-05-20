using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;
using RestaurantCarol.ViewModels;

namespace RestaurantCarol.Views
{
    public partial class AdreseView : Window
    {
        public event Action? AdreseModificate;

        private AdreseViewModel viewModel;
        private int idUtilizator;

        public AdreseView(int idUtilizator)
        {
            InitializeComponent();

            this.idUtilizator = idUtilizator;
            viewModel = new AdreseViewModel(idUtilizator);
            DataContext = viewModel;

            viewModel.AdreseModificate += () => AdreseModificate?.Invoke();
            viewModel.InchideRequested += () => Close();
            viewModel.DeschideAddPopupCerere += DeschideAddPopup;
            viewModel.DeschideEditPopupCerere += DeschideEditPopup;

            MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    DragMove();
            };
        }

        private Adresa? DeschideAddPopup()
        {
            AddEditAdresaView popup = new AddEditAdresaView(idUtilizator);
            popup.Owner = this;
            popup.ShowDialog();
            return popup.AdresaRezultat;
        }

        private Adresa? DeschideEditPopup(Adresa adresa)
        {
            AddEditAdresaView popup = new AddEditAdresaView(adresa);
            popup.Owner = this;
            popup.ShowDialog();
            return popup.AdresaRezultat;
        }
    }
}