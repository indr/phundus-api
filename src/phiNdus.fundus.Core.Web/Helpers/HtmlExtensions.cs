using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlString Button(this HtmlHelper htmlHelper, string caption)
        {
            return MvcHtmlString.Empty;
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

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption, string actionName)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, null);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption, string actionName,
                                                   object routedValues)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, routedValues, String.Empty);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
                                                   string actionName, object routedValues, string buttonIcon)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, routedValues, buttonIcon, ButtonClass.None);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
                                                   string actionName, object routedValues, string buttonIcon,
                                                   string buttonClass)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, null, routedValues, buttonIcon, buttonClass);
        }

        public static IHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string caption,
                                                   string actionName, string controllerName, object routedValues,
                                                   string buttonIcon)
        {
            return ActionLinkButton(htmlHelper, caption, actionName, controllerName, routedValues, buttonIcon,
                                    ButtonClass.None);
        }

        public static IHtmlString ActionLinkButton(HtmlHelper htmlHelper, string caption, string actionName,
                                                   string controllerName, object routedValues, string buttonIcon,
                                                   string buttonClass)
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
                                                                   Expression<Func<TModel, TValue>> expression,
                                                                   string legend)
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