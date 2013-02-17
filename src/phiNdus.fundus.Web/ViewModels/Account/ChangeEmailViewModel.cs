using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.ViewModels.Account
{
    public class ChangeEmailViewModel
    {
        [Required]
        [DisplayName("E-Mail-Adresse")]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        public string Email { get; set; }
    }
}