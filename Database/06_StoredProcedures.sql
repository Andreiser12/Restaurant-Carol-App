USE RestaurantCarol;
GO

CREATE OR ALTER PROCEDURE CheckEmailExists
    @email NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Utilizator WHERE Email = @email)
        SELECT 1 AS Exista;
    ELSE
        SELECT 0 AS Exista;
END
GO

CREATE OR ALTER PROCEDURE AddUtilizator
    @nume NVARCHAR(100),
    @prenume NVARCHAR(100),
    @email NVARCHAR(200),
    @telefon NVARCHAR(20),
    @adresaLivrare NVARCHAR(500),
    @parolaHash NVARCHAR(500),
    @rol NVARCHAR(20),
    @idUtilizator INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Utilizator (Nume, Prenume, Email, Telefon, AdresaLivrare, ParolaHash, Rol)
    VALUES (@nume, @prenume, @email, @telefon, @adresaLivrare, @parolaHash, @rol);
    
    SET @idUtilizator = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE UpdateStocPreparat
    @idPreparat INT,
    @cantitateTotala DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Preparat
    SET CantitateTotala = @cantitateTotala
    WHERE IdPreparat = @idPreparat;
END
GO

CREATE OR ALTER PROCEDURE GetComenziManager
    @doarActive BIT
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
    WHERE (@doarActive = 0) OR (LOWER(c.Stare) NOT IN ('livrata', 'anulata'))
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
    WHERE (@doarActive = 0) OR (LOWER(c.Stare) NOT IN ('livrata', 'anulata'));
END
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

CREATE OR ALTER PROCEDURE GetComenziLivrator
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
    WHERE LOWER(c.Stare) = 'a plecat la client'
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
    WHERE LOWER(c.Stare) = 'a plecat la client';
END
GO

CREATE OR ALTER PROCEDURE UpdateStareComanda
    @idComanda INT,
    @nouaStare NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Comanda
    SET Stare = @nouaStare
    WHERE IdComanda = @idComanda;
END
GO