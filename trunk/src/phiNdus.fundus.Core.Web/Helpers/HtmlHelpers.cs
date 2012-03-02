using System;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
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
        public static string DragAndDrop { get { return @"ui-icon-arrowthick-2-n-s"; } }
    }

    public static class Icon
    {
        public static string None { get { return @""; } }
        public static string Edit { get { return @"icon-pencil"; } }
        public static string Detail { get { return @"icon-search"; } }
        public static string Remove { get { return @"icon-trash"; } }
        public static string Print { get { return @"icon-print"; } }
        public static string DragAndDrop { get { return @"icon-arrowthick-2-n-s"; } }
    }

    public static class ButtonClass
    {
        public static string None { get { return @""; } }
        public static string Primary { get { return @"btn-primary"; } }
        public static string Info { get { return @"btn-info"; } }
        public static string Success { get { return @"btn-success"; } }
        public static string Warning { get { return @"btn-warning"; } }
        public static string Danger { get { return @"btn-danger"; } }
        public static string Inverse { get { return @"btn-inverse"; } }
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

        public static IHtmlString Icon(this HtmlHelper htmlHelper, string tooltip, string iconName, string cursor)
        {
            var spanBuilder = new TagBuilder("span");
            spanBuilder.AddCssClass(string.Format("ui-icon {0}", iconName));

            var divBuilder = new TagBuilder("div")
            {
                InnerHtml = spanBuilder.ToString(TagRenderMode.Normal)
            };
            divBuilder.AddCssClass("icon ui-state-default ui-corner-all");
            divBuilder.Attributes.Add("title", tooltip);
            // TODO: Pls Refactor. De Schneller esch de Gschwender!
            if (!String.IsNullOrWhiteSpace(cursor))
                divBuilder.Attributes.Add("style", "cursor: " + cursor);
            return MvcHtmlString.Create(divBuilder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString SubmitButton(this HtmlHelper htmlHelper, string caption)
        {
            return SubmitButton(htmlHelper, caption, ButtonClass.Primary);
        }

        public static IHtmlString SubmitButton(this HtmlHelper htmlHelper, string caption, string buttonClass)
        {
            //<input class="btn {buttonClass}" type="submit" value="Speichern" />
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("value", caption);
            builder.AddCssClass("btn");
            builder.AddCssClass(buttonClass);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static IHtmlString Button(this HtmlHelper htmlHelper, string caption)
        {
            return MvcHtmlString.Empty;
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption, string actionName)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, null);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption, string actionName, object routedValues)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, routedValues, String.Empty);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
            string actionName, object routedValues, string buttonIcon)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, routedValues, buttonIcon, ButtonClass.None);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
            string actionName, object routedValues, string buttonIcon, string buttonClass)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, null, routedValues, buttonIcon, buttonClass);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
            string actionName, string controllerName, object routedValues, string buttonIcon)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, controllerName, routedValues, buttonIcon, ButtonClass.None);
        }

        public static IHtmlString ActionLinkButton(HtmlHelper htmlHelper, string caption, string actionName, string controllerName, object routedValues, string buttonIcon, string buttonClass)
        {
            var innerHtml = caption;
            if (!String.IsNullOrWhiteSpace(buttonIcon))
            {
                var i = new TagBuilder("i");
                i.AddCssClass(buttonIcon);
                innerHtml = i.ToString(TagRenderMode.Normal);

                if (!String.IsNullOrWhiteSpace(caption))
                    innerHtml += @"&nbsp;" + caption; 
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var a = new TagBuilder("a");
            a.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routedValues));
            a.InnerHtml = innerHtml;
            a.AddCssClass("btn");
            a.AddCssClass(buttonClass);

            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }

        // http://stackoverflow.com/questions/989005/make-an-html-actionlink-around-an-image-in-asp-net-mvc
        public static IHtmlString ActionLinkIcon(this HtmlHelper htmlHelper, string tooltip, string iconName, string actionName, object routedValues)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

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

        public static MvcFieldSet BeginFieldSetFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string legend)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return new MvcFieldSet(htmlHelper.ViewContext, id, legend);
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
