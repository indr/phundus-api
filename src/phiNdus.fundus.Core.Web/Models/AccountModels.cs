using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;

namespace phiNdus.fundus.Core.Web.Models {

    #region Models

    public class LogOnModel {
        [Required]
        [DisplayName("E-Mail-Adresse")]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Passwort")]
        public string Password { get; set; }

        [DisplayName("Angemeldet bleiben")]
        public bool RememberMe { get; set; }

    }

    public class SignUpModel {
        [Required]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        [DisplayName("E-Mail-Adresse")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Vorname")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Nachname")]
        public string LastName { get; set; }
    }

    #endregion
    
    #region Attributes

    public class EmailAttribute : RegularExpressionAttribute {

        // http://weblogs.asp.net/scottgu/archive/2010/01/15/asp-net-mvc-2-model-validation.aspx
        public EmailAttribute()
            : base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$") {}
    }

    #endregion

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
            if (String.IsNullOrEmpty(email))
                throw new ArgumentException("Kann nicht leer sein", "email");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Kann nicht leer sein", "password");

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

}