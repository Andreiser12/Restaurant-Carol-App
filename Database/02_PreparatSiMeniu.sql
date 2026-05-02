USE RestaurantCarol;
GO

CREATE TABLE Preparat (
    IdPreparat INT PRIMARY KEY IDENTITY(1,1),
    Denumire NVARCHAR(200) NOT NULL,
    Pret DECIMAL(10,2) NOT NULL CHECK (Pret > 0),
    CantitatePortie DECIMAL(10,2) NOT NULL CHECK (CantitatePortie > 0),
    CantitateTotala DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (CantitateTotala >= 0),
    Descriere NVARCHAR(MAX) NULL,
    Calorii INT NULL,
    Grasimi DECIMAL(5,2) NULL,
    Carbohidrati DECIMAL(5,2) NULL,
    Proteine DECIMAL(5,2) NULL,
    Sare DECIMAL(5,2) NULL,
    IdCategorie INT NOT NULL,
    CONSTRAINT FK_Preparat_Categorie 
        FOREIGN KEY (IdCategorie) REFERENCES Categorie(IdCategorie)
);
GO

CREATE TABLE Meniu (
    IdMeniu INT PRIMARY KEY IDENTITY(1,1),
    Denumire NVARCHAR(200) NOT NULL,
    IdCategorie INT NOT NULL,
    CONSTRAINT FK_Meniu_Categorie 
        FOREIGN KEY (IdCategorie) REFERENCES Categorie(IdCategorie)
);
GO