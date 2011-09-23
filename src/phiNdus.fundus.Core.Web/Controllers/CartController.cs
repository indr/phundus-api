using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List() {
            return View();
        }

    }
}
