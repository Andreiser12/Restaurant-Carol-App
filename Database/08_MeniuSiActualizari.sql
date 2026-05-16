USE RestaurantCarol;
GO

IF COL_LENGTH('dbo.Comanda', 'CostTotal') IS NULL
    ALTER TABLE dbo.Comanda ADD CostTotal DECIMAL(10,2) NOT NULL DEFAULT 0;
GO

IF COL_LENGTH('dbo.ItemComanda', 'IdMeniu') IS NULL
    ALTER TABLE dbo.ItemComanda ADD IdMeniu INT NULL;
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'PlaseazaComanda')
    DROP PROCEDURE PlaseazaComanda;
GO
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'AddMeniu')
    DROP PROCEDURE AddMeniu;
GO
IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'ItemComandaType')
    DROP TYPE dbo.ItemComandaType;
GO
IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'MeniuPreparatType')
    DROP TYPE dbo.MeniuPreparatType;
GO

CREATE TYPE dbo.ItemComandaType AS TABLE
(
    IdPreparat INT NULL,
    IdMeniu INT NULL,
    Cantitate INT NOT NULL
);
GO

CREATE TYPE dbo.MeniuPreparatType AS TABLE
(
    IdPreparat INT NOT NULL,
    CantitatePortie DECIMAL(10,2) NOT NULL
);
GO

CREATE OR ALTER PROCEDURE PlaseazaComanda
    @idUtilizator INT,
    @idAdresaLivrare INT,
    @codComanda NVARCHAR(20),
    @oraEstimataLivrare DATETIME,
    @costMancare DECIMAL(10,2),
    @costTransport DECIMAL(10,2),
    @discount DECIMAL(10,2),
    @itemeComanda ItemComandaType READONLY,
    @idComanda INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @idPreparatProblema INT;
        DECLARE @denumireProblema NVARCHAR(200);
        DECLARE @msg NVARCHAR(500);

        SELECT TOP 1
            @idPreparatProblema = ic.IdPreparat,
            @denumireProblema = p.Denumire
        FROM @itemeComanda ic
        INNER JOIN Preparat p ON p.IdPreparat = ic.IdPreparat
        WHERE ic.IdPreparat IS NOT NULL
          AND p.CantitateTotala < (p.CantitatePortie * ic.Cantitate);

        IF @idPreparatProblema IS NOT NULL
        BEGIN
            ROLLBACK TRANSACTION;
            SET @msg = N'Stoc insuficient pentru preparatul: ' + @denumireProblema;
            THROW 51000, @msg, 1;
            RETURN;
        END

        SELECT TOP 1
            @idPreparatProblema = mp.IdPreparat,
            @denumireProblema = p.Denumire
        FROM @itemeComanda ic
        INNER JOIN MeniuPreparat mp ON mp.IdMeniu = ic.IdMeniu
        INNER JOIN Preparat p ON p.IdPreparat = mp.IdPreparat
        WHERE ic.IdMeniu IS NOT NULL
          AND p.CantitateTotala < mp.CantitatePortie * ic.Cantitate;

        IF @idPreparatProblema IS NOT NULL
        BEGIN
            ROLLBACK TRANSACTION;
            SET @msg = N'Stoc insuficient pentru meniu (componenta): ' + @denumireProblema;
            THROW 51000, @msg, 1;
            RETURN;
        END

        IF NOT EXISTS (
            SELECT 1 FROM AdresaLivrare
            WHERE IdAdresa = @idAdresaLivrare AND IdUtilizator = @idUtilizator
        )
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 51003, N'Adresa de livrare nu apartine utilizatorului.', 1;
            RETURN;
        END

        INSERT INTO Comanda (
            CodComanda, IdUtilizator, IdAdresaLivrare,
            DataComanda, OraEstimataLivrare, Stare,
            CostMancare, CostTransport, Discount, CostTotal
        )
        VALUES (
            @codComanda, @idUtilizator, @idAdresaLivrare,
            GETDATE(), @oraEstimataLivrare, N'inregistrata',
            @costMancare, @costTransport, @discount,
            @costMancare + @costTransport - @discount
        );

        SET @idComanda = SCOPE_IDENTITY();

        INSERT INTO ItemComanda (IdComanda, IdPreparat, IdMeniu, Cantitate)
        SELECT @idComanda, IdPreparat, IdMeniu, Cantitate
        FROM @itemeComanda;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
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

    IF LOWER(@nouaStare) = N'livrata'
    BEGIN
        UPDATE p
        SET p.CantitateTotala = p.CantitateTotala - (p.CantitatePortie * ic.Cantitate)
        FROM Preparat p
        INNER JOIN ItemComanda ic ON ic.IdPreparat = p.IdPreparat
        WHERE ic.IdComanda = @idComanda AND ic.IdPreparat IS NOT NULL;

        UPDATE p
        SET p.CantitateTotala = p.CantitateTotala - (mp.CantitatePortie * ic.Cantitate)
        FROM Preparat p
        INNER JOIN MeniuPreparat mp ON mp.IdPreparat = p.IdPreparat
        INNER JOIN ItemComanda ic ON ic.IdMeniu = mp.IdMeniu
        WHERE ic.IdComanda = @idComanda AND ic.IdMeniu IS NOT NULL;
    END
END
GO

CREATE OR ALTER PROCEDURE GetMeniuriByCategorie
    @idCategorie INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.IdMeniu, m.Denumire, m.IdCategorie
    FROM Meniu m
    WHERE m.IdCategorie = @idCategorie
    ORDER BY m.Denumire;

    SELECT mp.IdMeniu, mp.IdPreparat, mp.CantitatePortie,
           p.Denumire, p.Pret, p.CantitatePortie AS CantitatePortiePreparat, p.CantitateTotala
    FROM MeniuPreparat mp
    INNER JOIN Meniu m ON m.IdMeniu = mp.IdMeniu
    INNER JOIN Preparat p ON p.IdPreparat = mp.IdPreparat
    WHERE m.IdCategorie = @idCategorie;
END
GO

CREATE OR ALTER PROCEDURE AddMeniu
    @denumire NVARCHAR(200),
    @idCategorie INT,
    @componente MeniuPreparatType READONLY,
    @idMeniu INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Meniu (Denumire, IdCategorie)
    VALUES (@denumire, @idCategorie);

    SET @idMeniu = SCOPE_IDENTITY();

    INSERT INTO MeniuPreparat (IdMeniu, IdPreparat, CantitatePortie)
    SELECT @idMeniu, IdPreparat, CantitatePortie
    FROM @componente;
END
GO

-- Actualizare result set iteme comanda (preparat + meniu)
CREATE OR ALTER PROCEDURE GetComenziManager
    @doarActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.IdComanda, c.CodComanda, c.IdUtilizator, c.DataComanda, c.OraEstimataLivrare,
        c.CostMancare, c.CostTransport, c.Discount, c.Stare,
        u.Nume AS NumeClient, u.Prenume AS PrenumeClient, u.Telefon AS TelefonClient,
        ISNULL(a.Adresa, N'Adresa necunoscuta') AS AdresaLivrareCompleta
    FROM Comanda c
    INNER JOIN Utilizator u ON c.IdUtilizator = u.IdUtilizator
    LEFT JOIN AdresaLivrare a ON c.IdAdresaLivrare = a.IdAdresa
    WHERE (@doarActive = 0) OR (LOWER(c.Stare) NOT IN (N'livrata', N'anulata'))
    ORDER BY c.DataComanda DESC;
    SELECT ic.IdItemComanda, ic.IdComanda, ic.IdPreparat, ic.IdMeniu, ic.Cantitate,
        p.Denumire AS DenumirePreparat, m.Denumire AS DenumireMeniu
    FROM ItemComanda ic
    INNER JOIN Comanda c ON ic.IdComanda = c.IdComanda
    LEFT JOIN Preparat p ON ic.IdPreparat = p.IdPreparat
    LEFT JOIN Meniu m ON ic.IdMeniu = m.IdMeniu
    WHERE (@doarActive = 0) OR (LOWER(c.Stare) NOT IN (N'livrata', N'anulata'));
END
GO

CREATE OR ALTER PROCEDURE GetComenziClient
    @idUtilizator INT,
    @doarActive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.IdComanda, c.CodComanda, c.IdUtilizator, c.DataComanda, c.OraEstimataLivrare,
        c.CostMancare, c.CostTransport, c.Discount, c.Stare,
        u.Nume AS NumeClient, u.Prenume AS PrenumeClient, u.Telefon AS TelefonClient,
        ISNULL(a.Adresa, N'Adresa necunoscuta') AS AdresaLivrareCompleta
    FROM Comanda c
    INNER JOIN Utilizator u ON c.IdUtilizator = u.IdUtilizator
    LEFT JOIN AdresaLivrare a ON c.IdAdresaLivrare = a.IdAdresa
    WHERE c.IdUtilizator = @idUtilizator
      AND ((@doarActive = 0) OR (LOWER(c.Stare) NOT IN (N'livrata', N'anulata')))
    ORDER BY c.DataComanda DESC;
    SELECT ic.IdItemComanda, ic.IdComanda, ic.IdPreparat, ic.IdMeniu, ic.Cantitate,
        p.Denumire AS DenumirePreparat, m.Denumire AS DenumireMeniu
    FROM ItemComanda ic
    INNER JOIN Comanda c ON ic.IdComanda = c.IdComanda
    LEFT JOIN Preparat p ON ic.IdPreparat = p.IdPreparat
    LEFT JOIN Meniu m ON ic.IdMeniu = m.IdMeniu
    WHERE c.IdUtilizator = @idUtilizator
      AND ((@doarActive = 0) OR (LOWER(c.Stare) NOT IN (N'livrata', N'anulata')));
END
GO

CREATE OR ALTER PROCEDURE GetComenziLivrator
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.IdComanda, c.CodComanda, c.IdUtilizator, c.DataComanda, c.OraEstimataLivrare,
        c.CostMancare, c.CostTransport, c.Discount, c.Stare,
        u.Nume AS NumeClient, u.Prenume AS PrenumeClient, u.Telefon AS TelefonClient,
        ISNULL(a.Adresa, N'Adresa necunoscuta') AS AdresaLivrareCompleta
    FROM Comanda c
    INNER JOIN Utilizator u ON c.IdUtilizator = u.IdUtilizator
    LEFT JOIN AdresaLivrare a ON c.IdAdresaLivrare = a.IdAdresa
    WHERE LOWER(c.Stare) = N'a plecat la client'
    ORDER BY c.DataComanda DESC;
    SELECT ic.IdItemComanda, ic.IdComanda, ic.IdPreparat, ic.IdMeniu, ic.Cantitate,
        p.Denumire AS DenumirePreparat, m.Denumire AS DenumireMeniu
    FROM ItemComanda ic
    INNER JOIN Comanda c ON ic.IdComanda = c.IdComanda
    LEFT JOIN Preparat p ON ic.IdPreparat = p.IdPreparat
    LEFT JOIN Meniu m ON ic.IdMeniu = m.IdMeniu
    WHERE LOWER(c.Stare) = N'a plecat la client';
END
GO

CREATE OR ALTER PROCEDURE GetNrComenziClientInInterval
    @idUtilizator INT,
    @zile INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS NrComenzi
    FROM Comanda
    WHERE IdUtilizator = @idUtilizator
      AND LOWER(Stare) NOT IN (N'anulata')
      AND DataComanda >= DATEADD(DAY, -@zile, GETDATE());
END
GO
