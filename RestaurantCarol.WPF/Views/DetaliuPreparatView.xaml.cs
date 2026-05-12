using System.Windows;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class DetaliuPreparatView : Window
    {
        public DetaliuPreparatView()
        {
            InitializeComponent();
        }

        public DetaliuPreparatView(Preparat preparat) : this()
        {
            denumireText.Text = preparat.Denumire;
        }

        private void Inchide_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}