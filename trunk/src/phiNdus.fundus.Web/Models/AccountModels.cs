using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace phiNdus.fundus.Core.Web.Models
{
    public class LogOnModel
    {
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

    public class SignUpModel
    {
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

        [Required]
        [DisplayName("Passwort")]
        public string Password { get; set; }
    }


    // TODO: fundus-16
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class EmailAttribute : RegularExpressionAttribute
    {
        // http://weblogs.asp.net/scottgu/archive/2010/01/15/asp-net-mvc-2-model-validation.aspx
        public EmailAttribute()
            : base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")
        {
        }
    }
}