using System.Text.RegularExpressions;
using RestaurantCarol.Exceptions;
namespace RestaurantCarol.Layers
{
    public class RegisterBLL
    {
        private UtilizatorDAL utilizatorDAL = new UtilizatorDAL();
        private const int MIN_PAROLA_LENGTH = 6;
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);
        private static readonly Regex TelefonRegex = new(
            @"^\+?[0-9]{7,15}$",
            RegexOptions.Compiled);
        public Utilizator Register(string nume, string prenume, string email,
                            string telefon, string adresaLivrare,
                            string parola, string confirmaParola)
        {
            ValidateInput(nume, prenume, email, telefon, adresaLivrare, parola, confirmaParola);
            email = email.Trim().ToLower();
            nume = nume.Trim();
            prenume = prenume.Trim();
            telefon = telefon.Trim();
            adresaLivrare = adresaLivrare.Trim();
            if (utilizatorDAL.CheckEmailExists(email))
            {
                throw new RestaurantException("Exista deja un cont cu acest email.");
            }
            string parolaHash = BCrypt.Net.BCrypt.HashPassword(parola);
            Utilizator utilizator = new Utilizator
            {
                Nume = nume,
                Prenume = prenume,
                Email = email,
                Telefon = telefon,
                ParolaHash = parolaHash,
                Rol = RolUtilizator.Client
            };
            try
            {
                utilizatorDAL.AddUtilizator(utilizator);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new RestaurantException("Exista deja un cont cu acest email.");
            }
            AdresaBLL adresaBLL = new AdresaBLL();
            Adresa adresa = new Adresa
            {
                IdUtilizator = utilizator.IdUtilizator,
                AdresaText = adresaLivrare,
                EsteImplicita = true
            };
            try
            {
                adresaBLL.AddAdresa(adresa);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Eroare la inserare adresa pentru utilizator nou: {ex.Message}");
            }
            return utilizator;
        }
        private void ValidateInput(string nume, string prenume, string email,
                                   string telefon, string adresaLivrare,
                                   string parola, string confirmaParola)
        {
            if (string.IsNullOrWhiteSpace(nume))
                throw new RestaurantException("Numele este obligatoriu.");
            if (string.IsNullOrWhiteSpace(prenume))
                throw new RestaurantException("Prenumele este obligatoriu.");
            if (string.IsNullOrWhiteSpace(email))
                throw new RestaurantException("Email-ul este obligatoriu.");
            if (!EmailRegex.IsMatch(email.Trim()))
                throw new RestaurantException("Email-ul nu este valid.");
            if (string.IsNullOrWhiteSpace(telefon))
                throw new RestaurantException("Telefonul este obligatoriu.");
            if (!TelefonRegex.IsMatch(telefon.Trim()))
                throw new RestaurantException("Telefonul nu este valid (doar cifre, optional cu prefix +).");
            if (string.IsNullOrWhiteSpace(adresaLivrare))
                throw new RestaurantException("Adresa de livrare este obligatorie.");
            if (string.IsNullOrWhiteSpace(parola))
                throw new RestaurantException("Parola este obligatorie.");
            if (parola.Length < MIN_PAROLA_LENGTH)
                throw new RestaurantException($"Parola trebuie sa aiba minim {MIN_PAROLA_LENGTH} caractere.");
            if (parola != confirmaParola)
                throw new RestaurantException("Parolele nu coincid.");
        }
    }
}