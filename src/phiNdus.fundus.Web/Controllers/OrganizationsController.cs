namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    public class OrganizationsController : ControllerBase
    {
        [AllowAnonymous]
        public virtual ActionResult Index(Guid id)
        {
            return Redirect(@"/#/organizations/" + id);
        }
    }
}