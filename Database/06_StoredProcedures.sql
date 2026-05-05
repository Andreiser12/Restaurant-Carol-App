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