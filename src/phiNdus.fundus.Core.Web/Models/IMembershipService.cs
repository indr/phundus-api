using System.Web.Security;

namespace phiNdus.fundus.Core.Web.Models
{
    public interface IMembershipService {
        /// <summary>
        /// Prüft ob Benutzername und Passwort gültig sind
        /// </summary>
        /// <param name="email">E-Mail Adresse des Benutzers</param>
        /// <param name="password">Passwort des Benutzers</param>
        /// <returns>true wenn gültig</returns>
        bool ValidateUser(string email, string password);

        MembershipUser CreateUser(string email, string password, string firstName, string lastName, out MembershipCreateStatus status);
    }
}