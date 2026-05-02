USE RestaurantCarol;
GO

CREATE TABLE Fotografie (
    IdFotografie INT PRIMARY KEY IDENTITY(1,1),
    IdPreparat INT NOT NULL,
    CaleFisier NVARCHAR(500) NOT NULL,
    CONSTRAINT FK_Fotografie_Preparat 
        FOREIGN KEY (IdPreparat) REFERENCES Preparat(IdPreparat) ON DELETE CASCADE
);
GO

CREATE TABLE PreparatAlergen (
    IdPreparat INT NOT NULL,
    IdAlergen INT NOT NULL,
    CONSTRAINT PK_PreparatAlergen 
        PRIMARY KEY (IdPreparat, IdAlergen),
    CONSTRAINT FK_PreparatAlergen_Preparat 
        FOREIGN KEY (IdPreparat) REFERENCES Preparat(IdPreparat) ON DELETE CASCADE,
    CONSTRAINT FK_PreparatAlergen_Alergen 
        FOREIGN KEY (IdAlergen) REFERENCES Alergen(IdAlergen) ON DELETE CASCADE
);
GO

CREATE TABLE MeniuPreparat (
    IdMeniu INT NOT NULL,
    IdPreparat INT NOT NULL,
    CantitatePortie DECIMAL(10,2) NOT NULL CHECK (CantitatePortie > 0),
    CONSTRAINT PK_MeniuPreparat 
        PRIMARY KEY (IdMeniu, IdPreparat),
    CONSTRAINT FK_MeniuPreparat_Meniu 
        FOREIGN KEY (IdMeniu) REFERENCES Meniu(IdMeniu) ON DELETE CASCADE,
    CONSTRAINT FK_MeniuPreparat_Preparat 
        FOREIGN KEY (IdPreparat) REFERENCES Preparat(IdPreparat)
);
GO