using RestaurantCarol.Layers;
namespace RestaurantCarol.Views.Navigation
{
    public interface IMeniuRestaurantNavigator
    {
        void NavigateToHub();
        void NavigateToListaCategorii(TipCategorie tip, string titlu, string caleImagine);
        void NavigateToListaPreparate(Categorie categorie, TipCategorie tipParinte);
        void NavigateToListaPreparatePopulare();
        void ActualizeazaStareComanda(ComandaBLL.RezultatPlasareComanda rezultat);
        void ActualizeazaPuncte();
        System.Windows.Window? GetHostWindow();
    }
}
