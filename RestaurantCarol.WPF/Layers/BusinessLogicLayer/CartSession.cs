using System.Collections.ObjectModel;

namespace RestaurantCarol.Layers
{
    public static class CartSession
    {
        public static ObservableCollection<CartItem> Items { get; private set; } = [];

        public static event Action? CartChanged;

        public static int NumarTotalProduse
        {
            get
            {
                int total = 0;
                foreach (var item in Items)
                    total += item.Cantitate;
                return total;
            }
        }

        public static decimal CostTotal
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                    total += item.Subtotal;
                return total;
            }
        }

        public static bool EsteGol => Items.Count == 0;

        public static void AdaugaPreparat(Preparat preparat, int cantitate)
        {
            if (preparat == null || cantitate <= 0) return;
            if (!preparat.EsteDisponibil) return;

            CartItem? existing = null;
            foreach (var item in Items)
            {
                if (item.Preparat?.IdPreparat == preparat.IdPreparat)
                {
                    existing = item;
                    break;
                }
            }

            if (existing != null)
                existing.Cantitate += cantitate;
            else
                Items.Add(new CartItem { Preparat = preparat, Cantitate = cantitate });

            CartChanged?.Invoke();
        }

        public static void AdaugaMeniu(Meniu meniu, int cantitate)
        {
            if (meniu == null || cantitate <= 0) return;
            if (!meniu.EsteDisponibil) return;

            CartItem? existing = null;
            foreach (var item in Items)
            {
                if (item.Meniu?.IdMeniu == meniu.IdMeniu)
                {
                    existing = item;
                    break;
                }
            }

            if (existing != null)
                existing.Cantitate += cantitate;
            else
                Items.Add(new CartItem { Meniu = meniu, Cantitate = cantitate });

            CartChanged?.Invoke();
        }

        public static void ModificaCantitate(CartItem item, int cantitateNoua)
        {
            if (item == null) return;

            if (cantitateNoua <= 0)
            {
                Sterge(item);
                return;
            }

            item.Cantitate = cantitateNoua;
            CartChanged?.Invoke();
        }

        public static void Sterge(CartItem item)
        {
            if (item == null) return;

            Items.Remove(item);
            CartChanged?.Invoke();
        }

        public static void Goleste()
        {
            Items.Clear();
            CartChanged?.Invoke();
        }
    }
}