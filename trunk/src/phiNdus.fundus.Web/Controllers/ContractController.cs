using System.Web.Mvc;
using phiNdus.fundus.Web.ViewModels;

namespace phiNdus.fundus.Web.Controllers
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
        [Authorize(Roles = "Chief")]
        public ActionResult Signed()
        {
            return View("Signed", new ContractsViewModel());
        }

        //
        // GET: /Contract/Closed
        [Authorize(Roles = "Chief")]
        public ActionResult Closed()
        {
            return View("Closed", new ContractsViewModel());
        }
    }
}