namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using phiNdus.fundus.Web.ViewModels;

    [Authorize]
    public class ContractController : ControllerBase
    {
        //
        // GET: /Contract/
        [Transaction]
        public virtual ActionResult Index()
        {
            return View("My", new MyContractsViewModel());
        }

        //
        // GET: /Contract/Signed
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Signed()
        {
            return View("Signed", new ContractsViewModel());
        }

        //
        // GET: /Contract/Closed
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Closed()
        {
            return View("Closed", new ContractsViewModel());
        }
    }
}