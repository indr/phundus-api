using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace phiNdus.fundus.Web.ViewModels
{
    public class SendTestEmailViewModel : ViewModelBase
    {
        public SendTestEmailViewModel()
        {
            //TestBodyTemplate = "Dies ist ein Test!";
        }

        [DisplayName("Host")]
        public string TestHost { get; set; }

        [DisplayName("Login")]
        public string TestUserName { get; set; }

        [DisplayName("Passwort")]
        public string TestPassword { get; set; }

        [DisplayName("Absender")]
        public string TestFrom { get; set; }

        [DisplayName("Empfänger")]
        [Required]
        public string TestTo { get; set; }

        [DisplayName("Text")]
        [Required]
        public string TestBodyTemplate { get; set; }
    }
}