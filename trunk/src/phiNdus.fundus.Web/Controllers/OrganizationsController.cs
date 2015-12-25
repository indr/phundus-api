namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;

    public class OrganizationsController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index(string id)
        {
            return Redirect(@"/#/organizations/" + id);
        }
    }
}