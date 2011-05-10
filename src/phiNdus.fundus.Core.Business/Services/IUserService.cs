using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.Services {
    public interface IUserService {

        /// <summary>
        /// Liefert den Benutzer für die übergebene E-Mail Adresse.
        /// </summary>
        UserDto GetUser(string email);

        /// <summary>
        /// Erstelllt einen neuen Benutzer.
        /// </summary>
        UserDto CreateUser(string email, string password, string passwordQuestion, string passwordAnswer);

        /// <summary>
        /// Aktualisiert einen bestehenden Benutzer.
        /// </summary>
        void UpdateUser(UserDto user);

        /// <summary>
        /// Markiert einen User als gelöscht.
        /// </summary>
        bool DeleteUser(string email);

        /// <summary>
        /// Ändert das Passwort eines Benutzers.
        /// </summary>
        bool ChangePassword(string email, string oldPassword, string newPassword);

        /// <summary>
        /// Überprüft, ob ein Benutzer mit dem gegebenen Passwort existiert.
        /// </summary>
        bool ValidateUser(string email, string password);

        /// <summary>
        /// Setzt das Password für einen Benutzer zurück und liefert das neue Passwort.
        /// (Passwort wohl besser direkt per E-Mail versenden?)
        /// </summary>
        string ResetPassword(string email);
    }
}
