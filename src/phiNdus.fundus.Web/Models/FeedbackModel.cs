using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace phiNdus.fundus.Web.Models
{
    public class FeedbackModel
    {
        [Required]
        [Email(ErrorMessage = "Ungültige E-Mail-Adresse")]
        [DisplayName("E-Mail-Adresse")]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("Feedback")]
        public string Comment { get; set; }
    }
}