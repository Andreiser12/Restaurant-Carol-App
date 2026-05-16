USE RestaurantCarol;
GO

CREATE OR ALTER PROCEDURE GetComenziClient
    @idUtilizator INT,
    @doarActive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.IdComanda,
        c.CodComanda,
        c.IdUtilizator,
        c.DataComanda,
        c.OraEstimataLivrare,
        c.CostMancare,
        c.CostTransport,
        c.Discount,
        c.Stare,
        u.Nume AS NumeClient,
        u.Prenume AS PrenumeClient,
        u.Telefon AS TelefonClient,
        ISNULL(a.Adresa, 'Adresa necunoscuta') AS AdresaLivrareCompleta
    FROM Comanda c
    INNER JOIN Utilizator u ON c.IdUtilizator = u.IdUtilizator
    LEFT JOIN AdresaLivrare a ON c.IdAdresaLivrare = a.IdAdresa
    WHERE c.IdUtilizator = @idUtilizator
      AND ((@doarActive = 0) OR (LOWER(c.Stare) NOT IN ('livrata', 'anulata')))
    ORDER BY c.DataComanda DESC;

    SELECT 
        ic.IdItemComanda,
        ic.IdComanda,
        ic.IdPreparat,
        ic.Cantitate,
        p.Denumire AS DenumirePreparat
    FROM ItemComanda ic
    INNER JOIN Comanda c ON ic.IdComanda = c.IdComanda
    LEFT JOIN Preparat p ON ic.IdPreparat = p.IdPreparat
    WHERE c.IdUtilizator = @idUtilizator
      AND ((@doarActive = 0) OR (LOWER(c.Stare) NOT IN ('livrata', 'anulata')));
END
GO
