USE RestaurantCarol;
GO

CREATE TABLE Comanda (
    IdComanda INT PRIMARY KEY IDENTITY(1,1),
    CodComanda NVARCHAR(20) NOT NULL UNIQUE,
    IdUtilizator INT NOT NULL,
    DataComanda DATETIME NOT NULL DEFAULT GETDATE(),
    OraEstimataLivrare DATETIME NULL,
    Stare NVARCHAR(30) NOT NULL DEFAULT 'inregistrata' 
        CHECK (Stare IN ('inregistrata', 'se pregateste', 'a plecat la client', 'livrata', 'anulata')),
    CostMancare DECIMAL(10,2) NOT NULL DEFAULT 0,
    CostTransport DECIMAL(10,2) NOT NULL DEFAULT 0,
    Discount DECIMAL(10,2) NOT NULL DEFAULT 0,
    CostTotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Comanda_Utilizator 
        FOREIGN KEY (IdUtilizator) REFERENCES Utilizator(IdUtilizator)
);
GO

CREATE TABLE ItemComanda (
    IdItemComanda INT PRIMARY KEY IDENTITY(1,1),
    IdComanda INT NOT NULL,
    IdPreparat INT NULL,
    IdMeniu INT NULL,
    Cantitate INT NOT NULL CHECK (Cantitate > 0),
    CONSTRAINT FK_ItemComanda_Comanda 
        FOREIGN KEY (IdComanda) REFERENCES Comanda(IdComanda) ON DELETE CASCADE,
    CONSTRAINT FK_ItemComanda_Preparat 
        FOREIGN KEY (IdPreparat) REFERENCES Preparat(IdPreparat),
    CONSTRAINT FK_ItemComanda_Meniu 
        FOREIGN KEY (IdMeniu) REFERENCES Meniu(IdMeniu),
    CONSTRAINT CK_ItemComanda_PreparatSauMeniu 
        CHECK (
            (IdPreparat IS NOT NULL AND IdMeniu IS NULL) 
            OR 
            (IdPreparat IS NULL AND IdMeniu IS NOT NULL)
        )
);
GO