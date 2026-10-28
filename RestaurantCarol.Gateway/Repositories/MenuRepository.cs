using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantCarol.Domain.Entities;
using RestaurantCarol.Domain.Enums;
using RestaurantCarol.Domain.Interfaces;
using RestaurantCarol.Persistence;

namespace RestaurantCarol.Gateway.Repositories;

public class MenuRepository : BaseRepository, IMenuRepository
{
    public MenuRepository(RestaurantCarolDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetCategoriesAsync(CategoryType? type = null)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        if (type.HasValue)
        {
            command.CommandText = "GetCategoriiByTip";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@tip", type.Value.ToDbString()));
        }
        else
        {
            command.CommandText = "GetAllCategorii";
            command.CommandType = CommandType.StoredProcedure;
        }

        var list = new List<Category>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var catTypeString = reader.GetString(2);
            list.Add(new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = CategoryTypeExtensions.FromDbString(catTypeString),
                ImagePath = "images/carol_logo.png"
            });
        }

        return list;
    }

    public async Task<List<Dish>> GetDishesAsync(int? categoryId = null, string? search = null, List<int>? excludedAllergenIds = null)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        if (categoryId.HasValue)
        {
            command.CommandText = "GetPreparateByCategorie";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@idCategorie", categoryId.Value));
        }
        else
        {
            command.CommandText = "GetAllPreparate";
            command.CommandType = CommandType.StoredProcedure;
        }

        var dishes = new List<Dish>();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                dishes.Add(MapDish(reader));
            }
        }

        var allAllergens = await GetAllergensAsync();
        var allergenMap = allAllergens.ToDictionary(a => a.Id, a => a.Name);

        foreach (var dish in dishes)
        {
            foreach (var aId in dish.AllergenIds)
            {
                if (allergenMap.TryGetValue(aId, out var aName))
                {
                    dish.Allergens.Add(new Allergen { Id = aId, Name = aName });
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.Trim().ToLower();
            dishes = dishes.Where(d =>
                d.Name.ToLower().Contains(searchLower) ||
                (d.Description != null && d.Description.ToLower().Contains(searchLower))
            ).ToList();
        }

        if (excludedAllergenIds != null && excludedAllergenIds.Count > 0)
        {
            dishes = dishes.Where(d => !d.AllergenIds.Any(id => excludedAllergenIds.Contains(id))).ToList();
        }

        return dishes;
    }

    public async Task<List<Dish>> GetPopularDishesAsync(int top = 3)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("GetTopPreparatePopulare", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@top", top));

        var dishes = new List<Dish>();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                dishes.Add(MapDish(reader));
            }
        }

        if (dishes.Count == 0)
        {
            var all = await GetDishesAsync();
            dishes = all.Take(top).ToList();
        }

        return dishes;
    }

    public async Task<Dish?> GetDishByIdAsync(int id)
    {
        var all = await GetDishesAsync();
        return all.FirstOrDefault(d => d.Id == id);
    }

    public async Task<List<Allergen>> GetAllergensAsync()
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand("GetAllAlergeni", connection);
        command.CommandType = CommandType.StoredProcedure;

        var list = new List<Allergen>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Allergen
            {
                Id = reader.GetInt32(reader.GetOrdinal("IdAlergen")),
                Name = reader.GetString(reader.GetOrdinal("Denumire"))
            });
        }

        return list;
    }

    private static Dish MapDish(SqlDataReader reader)
    {
        var rawPath = reader.IsDBNull(reader.GetOrdinal("PrimaCalePoza"))
            ? null
            : reader.GetString(reader.GetOrdinal("PrimaCalePoza"));

        string? cleanPath = null;
        if (!string.IsNullOrEmpty(rawPath))
        {
            cleanPath = rawPath.TrimStart('/', '\\').Replace('\\', '/');
            if (cleanPath.StartsWith("Images/", StringComparison.OrdinalIgnoreCase))
            {
                cleanPath = "images/" + cleanPath.Substring(7);
            }
        }
        else
        {
            cleanPath = "images/carol_logo.png";
        }

        var allergenIds = new List<int>();
        var allergenOrdinal = -1;
        try { allergenOrdinal = reader.GetOrdinal("IdsAlergeni"); } catch { }

        if (allergenOrdinal >= 0 && !reader.IsDBNull(allergenOrdinal))
        {
            var str = reader.GetString(allergenOrdinal);
            var parts = str.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                if (int.TryParse(p.Trim(), out var parsedId))
                {
                    allergenIds.Add(parsedId);
                }
            }
        }

        return new Dish
        {
            Id = reader.GetInt32(reader.GetOrdinal("IdPreparat")),
            Name = reader.GetString(reader.GetOrdinal("Denumire")),
            Price = reader.GetDecimal(reader.GetOrdinal("Pret")),
            PortionSize = (int)reader.GetDecimal(reader.GetOrdinal("CantitatePortie")),
            TotalStock = (int)reader.GetDecimal(reader.GetOrdinal("CantitateTotala")),
            Description = reader.IsDBNull(reader.GetOrdinal("Descriere")) ? null : reader.GetString(reader.GetOrdinal("Descriere")),
            Calories = reader.IsDBNull(reader.GetOrdinal("Calorii")) ? null : reader.GetInt32(reader.GetOrdinal("Calorii")),
            Fats = reader.IsDBNull(reader.GetOrdinal("Grasimi")) ? null : reader.GetDecimal(reader.GetOrdinal("Grasimi")),
            Carbohydrates = reader.IsDBNull(reader.GetOrdinal("Carbohidrati")) ? null : reader.GetDecimal(reader.GetOrdinal("Carbohidrati")),
            Proteins = reader.IsDBNull(reader.GetOrdinal("Proteine")) ? null : reader.GetDecimal(reader.GetOrdinal("Proteine")),
            Salt = reader.IsDBNull(reader.GetOrdinal("Sare")) ? null : reader.GetDecimal(reader.GetOrdinal("Sare")),
            CategoryId = reader.GetInt32(reader.GetOrdinal("IdCategorie")),
            ImagePath = cleanPath,
            AllergenIds = allergenIds
        };
    }
}
