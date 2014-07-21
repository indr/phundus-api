namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Business.Services;
    using Castle.Transactions;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.Models.CartModels;
    using phiNdus.fundus.Web.ViewModels;
    using Phundus.Core.OrganisationCtx;

    public class ShopController : ControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }

        static string MasterView
        {
            get { return @"Index"; }
        }

        string ShopView
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

        string QueryString
        {
            get { return Convert.ToString(Session["Shop-QueryString"]); }
            set
            {
                if (value != null)
                    Session["Shop-QueryString"] = value;
            }
        }

        int? QueryOrganizationId
        {
            get { return Session["Shop-QueryOrganizationId"]  as int?; }
            set { Session["Shop-QueryOrganizationId"] = value; }
        }

        int? RowsPerPage
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

        [Transaction]
        public virtual ActionResult Index(int? page, FormCollection collection = null)
        {
            if (collection != null)
            {
                var value = collection.GetValue("PageSelectorModel.rowsPerPage");
                if (value != null)
                    RowsPerPage = Convert.ToInt32(value.AttemptedValue);
            }

            return Search(QueryString, QueryOrganizationId, page);
        }

        [Transaction]
        public virtual ActionResult Large()
        {
            ShopView = ShopViews.Large;
            return Index(null, null);
        }

        [Transaction]
        public virtual ActionResult Small()
        {
            ShopView = ShopViews.Small;
            return Index(null, null);
        }

        [Transaction]
        public virtual ActionResult Table()
        {
            ShopView = ShopViews.Table;
            return Index(null, null);
        }

        [Transaction]
        public virtual ActionResult Search(string queryString, int? queryOrganizationId, int? page)
        {
            if (page == null)
                page = 1;
            QueryString = queryString;
            QueryOrganizationId = queryOrganizationId;
            var model = new ShopSearchResultViewModel(
                QueryString,
                QueryOrganizationId,
                page.Value,
                RowsPerPage.Value,
                Organizations.FindAll());
            if (Request.IsAjaxRequest())
                return PartialView(ShopView, model);
            return View(ShopView, MasterView, model);
        }


        [Transaction]
        public virtual ActionResult Article(int id)
        {
            var model = new ShopArticleViewModel(id);

            model.Availabilities = ServiceLocator.Current.GetInstance<IArticleService>().GetAvailability(id);
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
        [Transaction]
        public virtual ActionResult AddToCart(CartItemModel item)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return new HttpStatusCodeResult(400);
                return RedirectToAction(ShopActionNames.Article, item.ArticleId);
            }

            var service = ServiceLocator.Current.GetInstance<ICartService>();
            var cart = service.AddItem(item.CreateDto());

            if (Request.IsAjaxRequest())
                return Json(cart);
            return RedirectToAction(CartActionNames.Index, ControllerNames.Cart);
        }

        #region Nested type: ShopViews

        static class ShopViews
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