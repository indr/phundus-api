namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Security;
    using Castle.Transactions;

    public class AuthController : ApiController
    {
        #region Default Actions

        //// GET api/auth
        //public IEnumerable<string> Get()
        //{
        //    return new[] {"value1", "value2"};
        //}

        //// GET api/auth/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/auth
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/auth/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/auth/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        
        [HttpPost]
        [Transaction]
        public virtual bool Login([FromBody] LogInDto value)
        {
            if (!Membership.ValidateUser(value.Username, value.Password))
                return false;

            FormsAuthentication.SetAuthCookie(value.Username, false);
            return true;
        }

        [HttpGet]
        [Transaction]
        public virtual bool Login(string username, string password)
        {
            if (!Membership.ValidateUser(username, password))
                return false;

            FormsAuthentication.SetAuthCookie(username, false);
            return true;
        }


        [HttpGet]
        [Authorize]
        public virtual string Secured()
        {
            return "This is secured for " + User.Identity.Name;
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