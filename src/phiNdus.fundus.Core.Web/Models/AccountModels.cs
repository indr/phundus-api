using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;

namespace phiNdus.fundus.Core.Web.Models {

    //=========================================================================================
    #region Models

    public class LogOnModel {
        [Required]
        [DisplayName("E-Mail Adresse")]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*", ErrorMessage = "Ungültige E-Mail Adresse")]
        public string EMail { get; set; }

        [Required]
        [DisplayName("Passwort")]
        public string Password { get; set; }

        [DisplayName("Angemeldet bleiben")]
        public bool RememberMe { get; set; }

    }

    #endregion
    //=========================================================================================

    //=========================================================================================
    #region Services

    public interface IMembershipService {
        /// <summary>
        /// Prüft ob Benutzername und Passwort gültig sind
        /// </summary>
        /// <param name="email">E-Mail Adresse des Benutzers</param>
        /// <param name="password">Passwort des Benutzers</param>
        /// <returns>true wenn gültig</returns>
        bool ValidateUser(string email, string password);
    }

    public class AccountMembershipService : IMembershipService {
        private readonly MembershipProvider _provider;

        public AccountMembershipService() {
            // Nutze den Default-Provider
            _provider = Membership.Provider;
        }

        public bool ValidateUser(string email, string password) {
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Kann nicht leer sein", "email");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Kann nicht leer sein", "password");

            return _provider.ValidateUser(email, password);
        }

    }

    public interface IFormsService {
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

    public class FormsAuthenticationService : IFormsService {
        public void SignIn(string email, bool createPersistendCookie) {
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Kann nicht leer sein", "username");

            FormsAuthentication.SetAuthCookie(email, createPersistendCookie);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }

    #endregion
    //=========================================================================================

}