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

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, int jsNumber, out MembershipCreateStatus status)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Kann nicht leer sein", "email");

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Kann nicht leer sein", "password");

            var provider = new FundusMembershipProvider();
            return provider.CreateUser(email, password, firstName, lastName, jsNumber, out status);
        }

        public bool ValidateValidationKey(string key)
        {
            var provider = new FundusMembershipProvider();
            return provider.ValidateValidationKey(key);
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            var provider = new FundusMembershipProvider();
            return provider.ChangePassword(email, oldPassword, newPassword);
        }

        #endregion
    }
}