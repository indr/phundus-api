﻿namespace Phundus.Rest.Controllers
{
    using System.Web.Http;
    using System.Web.Security;
    using Castle.Transactions;

    public class AuthController : ApiControllerBase
    {
        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual bool Login([FromBody] LogInDto value)
        {
            if (!Membership.ValidateUser(value.Username, value.Password))
                return false;

            FormsAuthentication.SetAuthCookie(value.Username, false);
            return true;
        }

        [HttpGet]
        [Transaction]
        [AllowAnonymous]
        public virtual bool Login(string username, string password)
        {
            if (!Membership.ValidateUser(username, password))
                return false;

            FormsAuthentication.SetAuthCookie(username, false);
            return true;
        }


        [HttpGet]
        public virtual string Secured()
        {
            return "This is secured for " + Identity.Name;
        }

        [HttpGet]
        public virtual string NotSecured()
        {
            return "This is public!";
        }

        [HttpGet]
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        #region Nested type: LogInDto

        public class LogInDto
        {
            public string Password { get; set; }
            public string Username { get; set; }
        }

        #endregion
    }
}