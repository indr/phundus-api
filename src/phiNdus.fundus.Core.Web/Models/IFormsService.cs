namespace phiNdus.fundus.Core.Web.Models
{
    public interface IFormsService
    {
        /// <summary>
        /// Speichert die Benutzerdaten in einer Session
        /// </summary>
        /// <param name="email">E-Mail Adresse des Benutzers</param>
        /// <param name="createPersistendCookie">Dauerhaftes Cookie</param>
        void SignIn(string email, bool createPersistendCookie);

        /// <summary>
        /// Löscht die Session und alle Sessiondaten
        /// </summary>
        void SignOut();
    }
}