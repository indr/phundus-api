namespace Phundus.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Authorization;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Inventory.Queries;
    using phiNdus.fundus.Web.ViewModels;
    using Shop.Authorization;
    using Shop.Orders.Model;
    using Shop.Queries;
    using Owner = Shop.Orders.Model.Owner;

    public class ShopController : ControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        public IShopArticleQueries ShopArticleQueries { get; set; }

        public IAvailabilityQueries AvailabilityQueries { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public IAuthorize Authorize { get; set; }

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

        private Guid? QueryOrganizationId
        {
            get { return Session["Shop-QueryOrganizationId"] as Guid?; }
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
        public virtual ActionResult Search(string queryString, Guid? queryOrganizationId, int? page)
        {
            if (page == null)
                page = 1;
            QueryString = queryString;
            QueryOrganizationId = queryOrganizationId;

            var organizations = OrganizationQueries.All();

            var model = new ShopSearchResultViewModel(
                QueryString,
                QueryOrganizationId,
                page.Value,
                RowsPerPage.Value,
                organizations,
                ShopArticleQueries);
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

            var model = new ShopArticleViewModel(article, CurrentUserId);


            if (Identity.IsAuthenticated)
            {
                //model.CanUserAddToCart = MemberInRole.IsActiveMember(model.Article.OrganizationId, new UserId(CurrentUserId));

                var adapted = new Article(article.Id,
                    new Owner(new OwnerId(article.OrganizationId), article.OrganizationName),
                    article.Name, article.Price);

                model.CanUserAddToCart = Authorize.Test(new UserId(CurrentUserId), Rent.Article(adapted)); ;
            }

            model.Availabilities = AvailabilityQueries.GetAvailability(id).ToList();
            return Json(new
            {
                caption = model.Article.Name,
                content = RenderPartialViewToString("Article", model)
            }, JsonRequestBehavior.AllowGet);
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