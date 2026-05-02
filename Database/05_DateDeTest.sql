-- ========================================
-- Script 05: Date de test
-- ========================================

USE RestaurantCarol;
GO

-- ===== CATEGORII =====
INSERT INTO Categorie (Denumire) VALUES 
    (N'Mic Dejun'),
    (N'Starter'),
    (N'Salate'),
    (N'Ciorbe'),
    (N'Finger Food'),
    (N'Preparate din carne de pasare'),
    (N'Preparate din carne de porc'),
    (N'Meniu Turcesc'),
    (N'Produse Traditionale'),
    (N'Peste si Fructe de Mare'),
    (N'Pasta & More'),
    (N'Garnituri'),
    (N'Salate de insotire'),
    (N'Desert'),
    (N'Pizza'),
    (N'Extra'),
    (N'Bauturi');
GO

-- ===== ALERGENI =====
INSERT INTO Alergen (Denumire) VALUES 
    (N'Gluten'),
    (N'Oua'),
    (N'Lapte'),
    (N'Telina'),
    (N'Mustar'),
    (N'Susan'),
    (N'Soia'),
    (N'Peste'),
    (N'Crustacee'),
    (N'Moluste'),
    (N'Arahide'),
    (N'Fructe cu coaja lemnoasa'),
    (N'Sulfati'),
    (N'Lupin');
GO

-- ===== PREPARATE =====
-- Ciorba de pui a la grec (Ciorbe = IdCategorie 4)
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, Descriere, Calorii, Grasimi, Carbohidrati, Proteine, Sare, IdCategorie)
VALUES (
    N'Ciorba de pui a la grec', 
    28.50, 380, 5000,
    N'Ciorba de pui cu smantana, ou, legume, orez jasmine, lamaie',
    300, 18.0, 15.0, 20.0, 2.1,
    4
);

-- Salata greceasca (Salate = IdCategorie 3)
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, Descriere, Calorii, Grasimi, Carbohidrati, Proteine, Sare, IdCategorie)
VALUES (
    N'Salata greceasca', 
    24.00, 350, 3000,
    N'Rosii, castraveti, ardei, ceapa rosie, masline, branza feta, ulei de masline',
    280, 22.0, 12.0, 8.0, 1.8,
    3
);

-- Piept de pui la gratar (Preparate din carne de pasare = IdCategorie 6)
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, Descriere, Calorii, Grasimi, Carbohidrati, Proteine, Sare, IdCategorie)
VALUES (
    N'Piept de pui la gratar', 
    32.00, 250, 4000,
    N'Piept de pui marinat, condimente, servit cu garnitura la alegere',
    320, 8.0, 2.0, 45.0, 1.5,
    6
);

-- Cartofi prajiti (Garnituri = IdCategorie 12)
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, Descriere, Calorii, Grasimi, Carbohidrati, Proteine, Sare, IdCategorie)
VALUES (
    N'Cartofi prajiti', 
    12.00, 200, 8000,
    N'Cartofi prajiti crocanti, sare de mare',
    350, 15.0, 48.0, 5.0, 1.2,
    12
);

-- Pastrav la gratar (Peste si Fructe de Mare = IdCategorie 10)
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, Descriere, Calorii, Grasimi, Carbohidrati, Proteine, Sare, IdCategorie)
VALUES (
    N'Pastrav la gratar', 
    45.00, 300, 2000,
    N'Pastrav proaspat la gratar, lamaie, ierburi aromatice',
    420, 22.0, 0.0, 38.0, 1.6,
    10
);

-- Coca-Cola (Bauturi = IdCategorie 17) - fara valori nutritionale, fara descriere
INSERT INTO Preparat (Denumire, Pret, CantitatePortie, CantitateTotala, IdCategorie)
VALUES (N'Coca-Cola', 8.00, 330, 10000, 17);
GO

-- ===== ASOCIERI PREPARAT - ALERGEN =====
-- Ciorba de pui a la grec (Id 1): Oua (2), Lapte (3), Telina (4)
INSERT INTO PreparatAlergen (IdPreparat, IdAlergen) VALUES (1, 2), (1, 3), (1, 4);

-- Salata greceasca (Id 2): Lapte (3) - de la branza feta
INSERT INTO PreparatAlergen (IdPreparat, IdAlergen) VALUES (2, 3);

-- Piept de pui (Id 3): fara alergeni standard

-- Cartofi prajiti (Id 4): fara alergeni

-- Pastrav la gratar (Id 5): Peste (8)
INSERT INTO PreparatAlergen (IdPreparat, IdAlergen) VALUES (5, 8);

-- Coca-Cola (Id 6): fara alergeni
GO

-- ===== UN MENIU EXEMPLU =====
-- "Meniu Pastrav" in categoria Peste si Fructe de Mare (IdCategorie 10)
INSERT INTO Meniu (Denumire, IdCategorie) VALUES (N'Meniu Pastrav', 10);

-- Componentele meniului: Pastrav (Id 5) cu 300g + Cartofi prajiti (Id 4) cu 200g
-- IdMeniu va fi 1 (primul meniu inserat)
INSERT INTO MeniuPreparat (IdMeniu, IdPreparat, CantitatePortie) VALUES 
    (1, 5, 300),
    (1, 4, 200);
GO

-- ===== UTILIZATORI TEST =====
-- ATENTIE: parolele sunt deocamdata in clar cu prefix "PLAINTEXT_" 
-- Le vom inlocui cu hash-uri reale cand implementam autentificarea in C#
INSERT INTO Utilizator (Nume, Prenume, Email, Telefon, AdresaLivrare, ParolaHash, Rol)
VALUES 
    (N'Popescu', N'Ion', N'ion.popescu@test.ro', N'0721234567', N'Strada Mihai Eminescu 10, Braila', N'PLAINTEXT_parola123', N'Client'),
    (N'Ionescu', N'Maria', N'maria.ionescu@carol.ro', N'0731234567', NULL, N'PLAINTEXT_admin123', N'Angajat');
GO

-- ===== VERIFICARE =====
SELECT 'Categorii' AS Tabela, COUNT(*) AS NumarRanduri FROM Categorie
UNION ALL SELECT 'Alergeni', COUNT(*) FROM Alergen
UNION ALL SELECT 'Preparate', COUNT(*) FROM Preparat
UNION ALL SELECT 'PreparatAlergen', COUNT(*) FROM PreparatAlergen
UNION ALL SELECT 'Meniuri', COUNT(*) FROM Meniu
UNION ALL SELECT 'MeniuPreparat', COUNT(*) FROM MeniuPreparat
UNION ALL SELECT 'Utilizatori', COUNT(*) FROM Utilizator;
GO