namespace RestaurantCarol.Layers
{
    public class CatalogItem : BasePropertyChanged
    {
        public bool EsteMeniu { get; init; }
        public Preparat? Preparat { get; init; }
        public Meniu? Meniu { get; init; }

        public string Denumire => EsteMeniu ? Meniu?.Denumire ?? "" : Preparat?.Denumire ?? "";

        public string PrimaCalePoza => EsteMeniu
            ? "/Images/carol_logo.png"
            : (string.IsNullOrEmpty(Preparat?.PrimaCalePoza) ? "/Images/carol_logo.png" : Preparat!.PrimaCalePoza!);

        public bool EsteDisponibil => EsteMeniu
            ? Meniu?.EsteDisponibil ?? false
            : Preparat?.EsteDisponibil ?? false;

        public string EtichetaDisponibilitate => EsteDisponibil ? "" : "indisponibil";

        public decimal PretAfisat => EsteMeniu ? Meniu?.Pret ?? 0 : Preparat?.Pret ?? 0;

        public static CatalogItem DinPreparat(Preparat preparat) =>
            new() { EsteMeniu = false, Preparat = preparat };

        public static CatalogItem DinMeniu(Meniu meniu) =>
            new() { EsteMeniu = true, Meniu = meniu };
    }
}
