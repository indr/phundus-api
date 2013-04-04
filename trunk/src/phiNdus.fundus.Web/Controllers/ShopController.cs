using System;
using System.Web.Mvc;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Models.CartModels;
using phiNdus.fundus.Web.ViewModels;

namespace phiNdus.fundus.Web.Controllers
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ShopController : ControllerBase
    {
        private static string MasterView
        {
            get { return @"Index"; }
        }

        private string ShopView
        {
            get
            {
                var result = Convert.ToString(Session["Shop-View"]);
                if (!String.IsNullOrEmpty(result))
                    return result;
                return ShopViews.Default;
            }
            set { Session["Shop-View"] = value; }
        }

        private string Query
        {
            get { return Convert.ToString(Session["Shop-Query"]); }
            set
            {
                if (value != null)
                    Session["Shop-Query"] = value;
            }
        }

        private int? RowsPerPage
        {
            get
            {
                var value = Session["Shop-RowsPerPage"];
                if (value == null)
                    return 8;
                return Convert.ToInt32(value);
            }
            set
            {
                if (value != null)
                    Session["Shop-RowsPerPage"] = value;
            }
        }

        public ActionResult Index(int? page, FormCollection collection = null)
        {
            if (collection != null)
            {
                var value = collection.GetValue("PageSelectorModel.rowsPerPage");
                if (value != null)
                    RowsPerPage = Convert.ToInt32(value.AttemptedValue);
            }

            return Search(null, page.HasValue ? page.Value : 1);
        }

        public ActionResult Large()
        {
            ShopView = ShopViews.Large;
            return Index(null, null);
        }

        public ActionResult Small()
        {
            ShopView = ShopViews.Small;
            return Index(null, null);
        }

        public ActionResult Table()
        {
            ShopView = ShopViews.Table;
            return Index(null, null);
        }

        public ActionResult Search(string query, int page = 1)
        {
            Query = query;
            var model = new ShopSearchResultViewModel(Query, page, RowsPerPage.Value);
            if (Request.IsAjaxRequest())
                return PartialView(ShopView, model);
            return View(ShopView, MasterView, model);
        }

       
        public ActionResult Article(int id)
        {
            var model = new ShopArticleViewModel(id);
            
            model.Availabilities = GlobalContainer.Resolve<IArticleService>().GetAvailability(Session.SessionID, id);
            return Json(new
                            {
                                caption = model.Caption,
                                content = RenderPartialViewToString("Article", model)
                            }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Shop/AddToCart
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToCart(CartItemModel item)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return new HttpStatusCodeResult(400);
                return RedirectToAction(ShopActionNames.Article, item.ArticleId);
            }

            var service = GlobalContainer.Resolve<ICartService>();
            var cart = service.AddItem(Session.SessionID, item.CreateDto());

            if (Request.IsAjaxRequest())
                return Json(cart);
            return RedirectToAction(CartActionNames.Index, ControllerNames.Cart);
        }

        #region Nested type: ShopViews

        private static class ShopViews
        {
            public static string Large
            {
                get { return @"Result-Large"; }
            }

            public static string Small
            {
                get { return @"Result-Small"; }
            }

            public static string Table
            {
                get { return @"Result-Table"; }
            }

            public static string Default
            {
                get { return Large; }
            }
        }

        #endregion
    }
}