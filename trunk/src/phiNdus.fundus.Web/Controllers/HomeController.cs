using System;
using System.Web.Mvc;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            ViewData["IsAssigned"] = Users.IsSessionFactoryAssigned();
            return View();
        }

        public IUserRepository Users { get; set; }
    }
}