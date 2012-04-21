using System.Web.Mvc;
using phiNdus.fundus.Core.Web.ViewModels;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        //
        // GET: /Contract/
        public ActionResult Index()
        {
            return View("My", new MyContractsViewModel());
        }

        //
        // GET: /Contract/Signed
        [Authorize(Roles = "Admin")]
        public ActionResult Signed()
        {
            return View("Signed", new ContractsViewModel());
        }

        //
        // GET: /Contract/Closed
        [Authorize(Roles = "Admin")]
        public ActionResult Closed()
        {
            return View("Closed", new ContractsViewModel());
        }
    }
}