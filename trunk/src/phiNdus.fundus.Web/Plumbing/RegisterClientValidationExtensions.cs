using DataAnnotationsExtensions.ClientValidation;
using phiNdus.fundus.Web.Plumbing;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (RegisterClientValidationExtensions), "Start")]

namespace phiNdus.fundus.Web.Plumbing
{
    public static class RegisterClientValidationExtensions
    {
        public static void Start()
        {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }
    }
}