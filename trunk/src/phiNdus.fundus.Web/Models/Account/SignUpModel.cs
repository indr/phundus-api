using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Web.Models
{
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

        [Required]
        [Range(100000, 9999999)]
        [DisplayName("J+S-Nummer")]
        public int JsNumber { get; set; }

        [DisplayName("Verband")]
        public int? OrganizationId { get; set; }

        public IEnumerable<Organization> Organizations { get; set; }
    }
}