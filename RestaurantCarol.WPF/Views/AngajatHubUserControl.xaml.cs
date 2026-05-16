using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RestaurantCarol.Layers;

namespace RestaurantCarol.Views
{
    public partial class AngajatHubUserControl : UserControl
    {
        private AngajatHubView? parentView;

        public AngajatHubUserControl()
        {
            InitializeComponent();
        }

        public AngajatHubUserControl(AngajatHubView parent) : this()
        {
            parentView = parent;
            ConfigureazaCercuri();
        }

        private void ConfigureazaCercuri()
        {
            if (UserSession.CurrentUser == null) return;

            if (UserSession.IsBucatar)
            {
                ActiveazaButon(bucatarButton, bucatarImage);
                DezactiveazaButon(managerButton, managerImage);
                DezactiveazaButon(livratorButton, livratorImage);
            }
            else if (UserSession.IsManager)
            {
                DezactiveazaButon(bucatarButton, bucatarImage);
                ActiveazaButon(managerButton, managerImage);
                DezactiveazaButon(livratorButton, livratorImage);
            }
            else if (UserSession.IsLivrator)
            {
                DezactiveazaButon(bucatarButton, bucatarImage);
                DezactiveazaButon(managerButton, managerImage);
                ActiveazaButon(livratorButton, livratorImage);
            }
        }

        private void ActiveazaButon(Button btn, ImageBrush img)
        {
            btn.IsEnabled = true;
            btn.Cursor = Cursors.Hand;
            img.Opacity = 1;
        }

        private void DezactiveazaButon(Button btn, ImageBrush img)
        {
            btn.IsEnabled = false;
            btn.Cursor = Cursors.No;
            img.Opacity = 0.3;
        }

        private void Bucatar_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToBucatar();
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToManager();
        }

        private void Livrator_Click(object sender, RoutedEventArgs e)
        {
            parentView?.NavigateToLivrator();
        }
    }
}