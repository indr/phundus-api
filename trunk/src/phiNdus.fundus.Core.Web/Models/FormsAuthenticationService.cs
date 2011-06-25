using System;
using System.Web.Security;

namespace phiNdus.fundus.Core.Web.Models
{
    public class FormsAuthenticationService : IFormsService {
        public void SignIn(string email, bool createPersistendCookie) {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentException("Kann nicht leer sein", "email");

            FormsAuthentication.SetAuthCookie(email, createPersistendCookie);
        }

        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}