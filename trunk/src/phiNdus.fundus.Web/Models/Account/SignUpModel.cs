using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.Models
{
    public class SignUpModel
    {


        [Required]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        [DisplayName("E-Mail-Adresse")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Passwort")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Passwort (Wiederholung)")]
        [Compare("Password", ErrorMessage = "\"Passwort (Wiederholung)\" und \"Passwort\" stimmen nicht überein.")]
        public string PasswordAgain { get; set; }


        [Required]
        [DisplayName("Vorname")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Nachname")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Strasse")]
        public string Street { get; set; }

        [Required]
        [DisplayName("Postleitzahl")]
        public string Postcode { get; set; }

        [Required]
        [DisplayName("Ort")]
        public string City { get; set; }

        [Required]
        [DisplayName("Mobiltelefon")]
        public string MobilePhone { get; set; }


        [Required]
        [Range(1, 9999999999)]
        [DisplayName("J+S-Nummer / Infocard-Nummer")]
        public int JsNumber { get; set; }

        [Required]
        [DisplayName("Verband")]
        public int? OrganizationId { get; set; }

        public IEnumerable<Phundus.Core.OrganizationAndMembershipCtx.Model.Organization> Organizations { get; set; }
    }
}