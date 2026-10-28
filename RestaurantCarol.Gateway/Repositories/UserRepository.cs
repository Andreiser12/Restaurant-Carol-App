using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantCarol.Domain.Entities;
using RestaurantCarol.Domain.Enums;
using RestaurantCarol.Domain.Interfaces;
using RestaurantCarol.Persistence;

namespace RestaurantCarol.Gateway.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(RestaurantCarolDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"
            SELECT IdUtilizator, Nume, Prenume, Email, Telefon, ParolaHash, Rol, Puncte
            FROM Utilizator
            WHERE LOWER(Email) = LOWER(@email)";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@email", email.Trim()));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapUser(reader);
        }

        return null;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"
            SELECT IdUtilizator, Nume, Prenume, Email, Telefon, ParolaHash, Rol, Puncte
            FROM Utilizator
            WHERE IdUtilizator = @id";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@id", id));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapUser(reader);
        }

        return null;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string sql = "SELECT COUNT(1) FROM Utilizator WHERE LOWER(Email) = LOWER(@email)";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@email", email.Trim()));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO Utilizator (Nume, Prenume, Email, Telefon, ParolaHash, Rol, Puncte)
            VALUES (@nume, @prenume, @email, @telefon, @parolaHash, @rol, @puncte);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@nume", user.LastName));
        command.Parameters.Add(new SqlParameter("@prenume", user.FirstName));
        command.Parameters.Add(new SqlParameter("@email", user.Email.Trim().ToLower()));
        command.Parameters.Add(new SqlParameter("@telefon", (object?)user.PhoneNumber ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@parolaHash", user.PasswordHash));
        command.Parameters.Add(new SqlParameter("@rol", user.Role.ToDbString()));
        command.Parameters.Add(new SqlParameter("@puncte", user.LoyaltyPoints));

        var id = await command.ExecuteScalarAsync();
        return Convert.ToInt32(id);
    }

    private static User MapUser(SqlDataReader reader)
    {
        var roleString = reader.GetString(reader.GetOrdinal("Rol"));
        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("IdUtilizator")),
            LastName = reader.GetString(reader.GetOrdinal("Nume")),
            FirstName = reader.GetString(reader.GetOrdinal("Prenume")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("Telefon")) ? null : reader.GetString(reader.GetOrdinal("Telefon")),
            PasswordHash = reader.GetString(reader.GetOrdinal("ParolaHash")),
            Role = UserRoleExtensions.FromDbString(roleString),
            LoyaltyPoints = reader.GetInt32(reader.GetOrdinal("Puncte"))
        };
    }
}
