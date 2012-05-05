using System.Web.Security;

namespace phiNdus.fundus.Web.Models
{
    public interface IMembershipService {
        /// <summary>
        /// Prüft ob Benutzername und Passwort gültig sind
        /// </summary>
        /// <param name="email">E-Mail Adresse des Benutzers</param>
        /// <param name="password">Passwort des Benutzers</param>
        /// <returns>true wenn gültig</returns>
        bool ValidateUser(string email, string password);

        MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber, out MembershipCreateStatus status);

        /// <summary>
        /// Prüft ob der Validierungskey gültig ist und aktiviert den Benutzer.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>True</c>, wenn der Key gefunden und der Benutzer aktiviert werden konnte, andernfalls <c>False</c>.</returns>
        bool ValidateValidationKey(string key);
    }
}