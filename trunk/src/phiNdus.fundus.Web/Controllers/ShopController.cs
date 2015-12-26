﻿namespace Phundus.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Queries;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.Models.CartModels;
    using phiNdus.fundus.Web.ViewModels;

    public class ShopController : ControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        public IShopArticleQueries ShopArticleQueries { get; set; }

        public IAvailabilityQueries AvailabilityQueries { get; set; }

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

        private string QueryString
        {
            get { return Convert.ToString(Session["Shop-QueryString"]); }
            set
            {
                if (value != null)
                    Session["Shop-QueryString"] = value;
            }
        }

        private int? QueryOrganizationId
        {
            get { return Session["Shop-QueryOrganizationId"] as int?; }
            set { Session["Shop-QueryOrganizationId"] = value; }
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

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public virtual ActionResult Large()
        {
            ShopView = ShopViews.Large;
            return Index(null, null);
        }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Small()
        {
            ShopView = ShopViews.Small;
            return Index(null, null);
        }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Table()
        {
            ShopView = ShopViews.Table;
            return Index(null, null);
        }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Search(string queryString, int? queryOrganizationId, int? page)
        {
            if (page == null)
                page = 1;
            QueryString = queryString;
            QueryOrganizationId = queryOrganizationId;

            var organizations = OrganizationQueries.AllNonFree();

            var model = new ShopSearchResultViewModel(
                QueryString,
                QueryOrganizationId,
                page.Value,
                RowsPerPage.Value,
                organizations);
            if (Request.IsAjaxRequest())
                return PartialView(ShopView, model);
            return View(ShopView, MasterView, model);
        }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Article(int id)
        {
            var article = ShopArticleQueries.GetArticle(id);
            if (article == null)
                throw new HttpNotFoundException();

            var model = new ShopArticleViewModel(article);

            
            if (Identity.IsAuthenticated)
            {
                var currentUserId = CurrentUserId;
                model.CanUserAddToCart = MemberInRole.IsActiveMember(model.Article.OrganizationId,
                    currentUserId);
            }

            model.Availabilities = AvailabilityQueries.GetAvailability(id).ToList();
            return Json(new
            {
                caption = model.Article.Name,
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

            var userId = CurrentUserId;
            var service = ServiceLocator.Current.GetInstance<ICartService>();
            var cart = service.GetCartByUserId(userId);

            int? cartId = null;
            if (cart != null)
                cartId = cart.Id;
            cart = service.AddItem(cartId, userId, item.CreateDto());

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