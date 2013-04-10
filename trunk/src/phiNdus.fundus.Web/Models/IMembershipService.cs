namespace phiNdus.fundus.Web.Models
{
    using System.Web.Security;

    public interface IMembershipService
    {
        MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber,
                                  int? organizationId, out MembershipCreateStatus status);

        /// <summary>
        /// Prüft ob der Validierungskey gültig ist und aktiviert den Benutzer.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>True</c>, wenn der Key gefunden und der Benutzer aktiviert werden konnte, andernfalls <c>False</c>.</returns>
        bool ValidateValidationKey(string key);


        bool ValidateEmailKey(string key);
        bool ResetPassword(string email);
    }
}