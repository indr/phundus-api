using System.Web.Mvc;
using phiNdus.fundus.Core.Web.ViewModels;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class ShopController : ControllerBase
    {
        private static string MasterView
        {
            get { return @"Index"; }
        }

        public ActionResult Index()
        {
            var model = new ShopSearchResultViewModel();
            return View("Result-Table", MasterView, model);
        }

        public ActionResult Search(string query)
        {
            var model = new ShopSearchResultViewModel(query);
            if (Request.IsAjaxRequest())
                return PartialView("Result-Table", model);
            return View("Result-Table", MasterView, model);
        }

        public ActionResult Article(int id)
        {
            var model = new ArticleViewModel(id);

            return Json(new
                            {
                                caption = model.Caption,
                                content = RenderPartialViewToString("Article", model)
                            }, JsonRequestBehavior.AllowGet);
        }
    }
}