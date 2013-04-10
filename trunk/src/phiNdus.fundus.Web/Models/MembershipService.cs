using System;
using System.Web.Security;
using phiNdus.fundus.Web.Security;

namespace phiNdus.fundus.Web.Models
{
    public class MembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public MembershipService() : this(Membership.Provider)
        {
        }

        public MembershipService(MembershipProvider provider)
        {
            _provider = provider;
        }

        #region IMembershipService Members

        public bool ValidateUser(string email, string password)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Kann nicht leer sein", "email");

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Kann nicht leer sein", "password");

            return _provider.ValidateUser(email, password);
        }

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber, int? organizationId, out MembershipCreateStatus status)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Kann nicht leer sein", "email");

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Kann nicht leer sein", "password");

            var provider = new CustomMembershipProvider();
            return provider.CreateUser(email, password, firstName, lastName, jsNumber, organizationId, out status);
        }

        public bool ValidateValidationKey(string key)
        {
            var provider = new CustomMembershipProvider();
            return provider.ValidateValidationKey(key);
        }

        public bool ValidateEmailKey(string key)
        {
            var provider = new CustomMembershipProvider();
            return provider.ValidateEmailKey(key);
        }

        public bool ResetPassword(string email)
        {
            var provider = new CustomMembershipProvider();
            return provider.ResetPassword(email, null) != null;
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            var provider = new CustomMembershipProvider();
            return provider.ChangePassword(email, oldPassword, newPassword);
        }

        public bool ChangeEmailAddress(string email, string newEmail)
        {
            var provider = new CustomMembershipProvider();
            return provider.ChangeEmail(email, newEmail);
        }

        #endregion
    }
}