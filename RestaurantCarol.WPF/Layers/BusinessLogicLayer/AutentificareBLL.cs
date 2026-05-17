using RestaurantCarol.Exceptions;
namespace RestaurantCarol.Layers
{
    public class AutentificareBLL
    {
        private UtilizatorDAL utilizatorDAL = new UtilizatorDAL();
        public Utilizator Autentificare(string email, string parola)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new RestaurantException("Email-ul este obligatoriu.");
            }
            if (string.IsNullOrWhiteSpace(parola))
            {
                throw new RestaurantException("Parola este obligatorie.");
            }
            Utilizator? utilizator = utilizatorDAL.GetByEmail(email.Trim().ToLower());
            if (utilizator == null)
            {
                throw new RestaurantException("Email sau parola incorecta.");
            }
            bool parolaCorecta = BCrypt.Net.BCrypt.Verify(parola, utilizator.ParolaHash);
            if (!parolaCorecta)
            {
                throw new RestaurantException("Email sau parola incorecta.");
            }
            return utilizator;
        }
        public void DirectLogin(Utilizator utilizator)
        {
            if(utilizator==null)
            {
                throw new RestaurantException("Utilizator invalid.");
            }
            UserSession.Login(utilizator);
        }
        public int GetPuncteByUtilizator(int idUtilizator)
        {
            return utilizatorDAL.GetPuncteByUtilizator(idUtilizator);
        }
    }
}