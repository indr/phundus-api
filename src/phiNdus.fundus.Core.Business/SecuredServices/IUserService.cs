using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public interface IUserService
    {
        /// <summary>
        /// Liefert den Benutzer für die übergebene E-Mail Adresse.
        /// </summary>
        UserDto GetUser(string sessionKey, string email);

        /// <summary>
        /// Erstelllt einen neuen Benutzer.
        /// </summary>
        UserDto CreateUser(string sessionKey, string email, string password);

        /// <summary>
        /// Aktualisiert einen bestehenden Benutzer.
        /// </summary>
        void UpdateUser(string sessionKey, UserDto user);

        /// <summary>
        /// Markiert einen User als gelöscht.
        /// </summary>
        bool DeleteUser(string sessionKey, string email);

        /// <summary>
        /// Ändert das Passwort eines Benutzers.
        /// </summary>
        bool ChangePassword(string sessionKey, string email, string oldPassword, string newPassword);

        /// <summary>
        /// Überprüft, ob ein Benutzer mit dem gegebenen Passwort existiert
        /// und liefert einen gültigen Session-Key zurück
        /// </summary>
        string ValidateUser(string email, string password);

        /// <summary>
        /// Setzt das Password für einen Benutzer zurück und liefert das neue Passwort.
        /// </summary>
        string ResetPassword(string sessionKey, string email);
    }
}