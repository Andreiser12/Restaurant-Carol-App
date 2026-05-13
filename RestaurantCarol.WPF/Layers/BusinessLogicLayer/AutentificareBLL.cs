using RestaurantCarol.Exceptions;

namespace RestaurantCarol.Layers
{
    public class AutentificareBLL
    {
        private UtilizatorDAL utilizatorDAL = new UtilizatorDAL();

        public Utilizator Autentificare(string email, string parola, RolUtilizator rolAsteptat)
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

            if (utilizator.Rol != rolAsteptat)
            {
                if (rolAsteptat == RolUtilizator.Angajat)
                {
                    throw new RestaurantException("Acest cont nu are drepturi de angajat.");
                }
                else
                {
                    throw new RestaurantException("Acest cont nu este de tip client.");
                }
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
    }
}