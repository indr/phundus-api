using System;
using System.Web.Security;

namespace phiNdus.fundus.Core.Web.Models
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

        public MembershipUser CreateUser(string email, string password, out MembershipCreateStatus status)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Kann nicht leer sein", "email");

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Kann nicht leer sein", "password");

            return _provider.CreateUser(email, password, email, null, null, false, null, out status);
        }

        #endregion
    }
}