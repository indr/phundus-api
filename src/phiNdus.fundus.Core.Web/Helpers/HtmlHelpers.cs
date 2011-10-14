using System;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Helpers
{
    // http://jquery-ui.googlecode.com/svn/trunk/tests/static/icons.html
    public static class ActionLinkIcon
    {
        public static string Edit { get { return @"ui-icon-pencil"; } }
        public static string Detail { get { return @"ui-icon-search"; } }
        public static string Remove { get { return @"ui-icon-trash"; } }
        public static string Print { get { return @"ui-icon-print"; } }
    }

    public static class HtmlHelpers
    {
        public static IHtmlString LinkToRemoveProperty(this HtmlHelper htmlHelper, string linkText, string container)
        {
            var js = string.Format("javascript:removeProperty(this,'{0}');return false;", container);
            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", "#");
            tagBuilder.Attributes.Add("onclick", js);
            tagBuilder.InnerHtml = linkText;
            var tag = tagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        // http://stackoverflow.com/questions/989005/make-an-html-actionlink-around-an-image-in-asp-net-mvc
        public static IHtmlString ActionLinkIcon(this HtmlHelper htmlHelper, string tooltip, string iconName, string actionName, object routedValues)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            /* <a link="controller/action/..">
             *     <div class="icon ui-state-default ui-corner-all">
             *         <span class="ui-icon ui-icon-trash"></span>
             *     </div>
             * </a>
             */

            var spanBuilder = new TagBuilder("span");
            spanBuilder.AddCssClass(string.Format("ui-icon {0}", iconName));

            var divBuilder = new TagBuilder("div") {
                InnerHtml = spanBuilder.ToString(TagRenderMode.Normal)
            };
            divBuilder.AddCssClass("icon ui-state-default ui-corner-all");

            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = divBuilder.ToString(TagRenderMode.Normal)
            };
            tagBuilder.MergeAttribute("href", urlHelper.Action(actionName, routedValues));
            tagBuilder.Attributes.Add("title", tooltip);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));            
        }

        // http://stackoverflow.com/questions/3065307/client-id-for-property-asp-net-mvc
        public static MvcHtmlString IdFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var inputFieldId = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return MvcHtmlString.Create(inputFieldId);
        } 

        public static MvcHtmlString IdForProp<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return MvcHtmlString.Create(id);
        }

        public static MvcHtmlString NameFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            return MvcHtmlString.Create(name);
        }

        public static MvcContainer BeginContainerFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return new MvcContainer(htmlHelper.ViewContext, id);
        }

        public static MvcHtmlString DisplayVersion(this HtmlHelper htmlHelper)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString(3);
            var revision = assembly.GetName().Version.Revision;
            return MvcHtmlString.Create(String.Format("{0} (rev {1})", version, revision));
        }
    }
}
