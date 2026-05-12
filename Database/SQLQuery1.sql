USE RestaurantCarol;
GO

DECLARE @idHummus INT;

INSERT INTO Preparat (
    Denumire, 
    Pret, 
    CantitatePortie, 
    CantitateTotala, 
    Descriere, 
    Calorii, 
    Grasimi,
    IdCategorie
)
VALUES (
    N'Hummus cu fasii de vita',
    56.50,
    420,
    4200,
    N'Hummus din năut și tahini, cu ulei de măsline, zeamă de lămâie și usturoi, completat cu fâșii fragede de vită, rodie, sumac și susan, servit cu lipie.',
    720,
    42,
    2
);

SET @idHummus = SCOPE_IDENTITY();

INSERT INTO PreparatAlergen (IdPreparat, IdAlergen) VALUES
    (@idHummus, 1),
    (@idHummus, 2),
    (@idHummus, 3), 
    (@idHummus, 8),
    (@idHummus, 5);

INSERT INTO Fotografie (IdPreparat, CaleFisier) VALUES
    (@idHummus, '/Images/hummus.png');
GO

select * from Categorie order by IdCategorie

select * from Preparat where IdCategorie=2
order by IdPreparat

select * from Comanda