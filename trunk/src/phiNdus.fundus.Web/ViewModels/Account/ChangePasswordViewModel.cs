using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DisplayName("Altes Passwort")]
        public string OldPassword { get; set; }

        [Required]
        [DisplayName("Neues Passwort")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "\"Neues Passwort (Wiederholung)\" und \"Neues Passwort\" stimmen nicht überein.")]
        [DisplayName("Neues Passwort (Wiederholung)")]
        public string NewPasswordAgain { get; set; }
    }
}