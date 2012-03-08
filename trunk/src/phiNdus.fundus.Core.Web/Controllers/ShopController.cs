using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.State;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class ShopController : ControllerBase
    {
        private static string MasterView
        {
            get { return @"Index"; }
        }

        private static class ShopViews
        {
            public static string Large { get { return @"Result-Large"; } }
            public static string Small { get { return @"Result-Small"; } }
            public static string Table { get { return @"Result-Table"; } }
            public static string Default { get { return Large; } }
        }

        private string ShopView
        {
            get {
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
            var model = new ArticleViewModel(id);

            return Json(new
                            {
                                caption = model.Caption,
                                content = RenderPartialViewToString("Article", model)
                            }, JsonRequestBehavior.AllowGet);
        }
    }
}