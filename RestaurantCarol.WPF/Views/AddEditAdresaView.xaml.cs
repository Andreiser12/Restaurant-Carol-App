using System.Windows;
using System.Windows.Input;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class AddEditAdresaView : Window
    {
        public Adresa? AdresaRezultat { get; private set; }

        private Adresa? adresaEditata;

        public AddEditAdresaView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }

        public AddEditAdresaView(int idUtilizator) : this()
        {
            titluText.Text = "Adauga adresa noua";
            adresaTextBox.Text = "";
            adresaEditata = new Adresa
            {
                IdUtilizator = idUtilizator,
                AdresaText = "",
                EsteImplicita = false
            };
        }

        public AddEditAdresaView(Adresa adresaDeEditat) : this()
        {
            titluText.Text = "Modifica adresa";
            adresaTextBox.Text = adresaDeEditat.AdresaText;
            adresaEditata = adresaDeEditat;
        }

        private void Salveaza_Click(object sender, RoutedEventArgs e)
        {
            if (adresaEditata == null) return;

            string textNou = adresaTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(textNou))
            {
                MessageBox.Show("Adresa nu poate fi goala.",
                    "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (textNou.Length < 5)
            {
                MessageBox.Show("Adresa trebuie sa aiba minim 5 caractere.",
                    "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            adresaEditata.AdresaText = textNou;
            AdresaRezultat = adresaEditata;

            this.DialogResult = true;
            this.Close();
        }

        private void Anuleaza_Click(object sender, RoutedEventArgs e)
        {
            AdresaRezultat = null;
            this.DialogResult = false;
            this.Close();
        }
    }
}