namespace Phundus.Rest.Controllers
{
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Http;
    using System.Web.Security;
    using Castle.Transactions;

    public class SessionsController : ApiControllerBase
    {
        [Transaction]
        [AllowAnonymous]
        public virtual void Post(SessionDoc doc)
        {
            if (!Membership.ValidateUser(doc.Username, doc.Password))
                throw new AuthenticationException();

            FormsAuthentication.SetAuthCookie(doc.Username, false);           
        }

        [Transaction]
        [AllowAnonymous]
        public virtual void Delete()
        {
            FormsAuthentication.SignOut();
        }

        public class SessionDoc
        {
            public string Password { get; set; }
            public string Username { get; set; }
        }
    }
}