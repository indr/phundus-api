namespace Phundus.Rest.Controllers
{
    using System.Security.Authentication;
    using System.Web.Http;
    using System.Web.Security;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;

    [RoutePrefix("/api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        [AllowAnonymous]
        public virtual void Post(SessionDoc doc)
        {
            if (!Membership.ValidateUser(doc.Username, doc.Password))
                throw new AuthenticationException();

            FormsAuthentication.SetAuthCookie(doc.Username, false);
        }

        [DELETE("")]
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