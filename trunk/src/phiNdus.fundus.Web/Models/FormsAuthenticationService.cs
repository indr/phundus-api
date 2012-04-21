using System;
using System.Web.Security;

namespace phiNdus.fundus.Web.Models
{
    public class FormsAuthenticationService : IFormsService
    {
        #region IFormsService Members

        public void SignIn(string email, bool createPersistendCookie)
        {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            FormsAuthentication.SetAuthCookie(email, createPersistendCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}