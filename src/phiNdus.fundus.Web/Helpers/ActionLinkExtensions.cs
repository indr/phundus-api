using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace phiNdus.fundus.Web.Helpers
{
    public class Appearance
    {
        public string Caption { get; set; }
        public Button Button { get; set; }
    }

    public static class AjaxLinkExtensions
    {
        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, AjaxOptions ajaxOptions)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, object routeValues, AjaxOptions ajaxOptions)
        {
            return ajaxHelper.ActionLink(appearance, actionName, new RouteValueDictionary(routeValues), ajaxOptions, new Dictionary<string, object>());
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, AjaxOptions ajaxOptions)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            return ajaxHelper.ActionLink(appearance, actionName, new RouteValueDictionary(routeValues), ajaxOptions, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            AddButtonClasses(appearance, htmlAttributes);

            return ajaxHelper.ActionLink(appearance.Caption, actionName, routeValues, ajaxOptions, htmlAttributes);
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, Appearance appearance, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            throw new NotImplementedException();
        }

        private static void AddButtonClasses(Appearance appearance, IDictionary<string, object> htmlAttributes)
        {
            if (appearance.Button != Button.None)
            {
                switch (appearance.Button)
                {
                    case Button.None:
                        break;
                    case Button.Normal:
                        htmlAttributes.Add("class", "btn");
                        break;
                    case Button.Primary:
                        htmlAttributes.Add("class", "btn btn-primary");
                        break;
                    case Button.Info:
                        htmlAttributes.Add("class", "btn btn-info");
                        break;
                    case Button.Success:
                        htmlAttributes.Add("class", "btn btn-success");
                        break;
                    case Button.Warning:
                        htmlAttributes.Add("class", "btn btn-warning");
                        break;
                    case Button.Danger:
                        htmlAttributes.Add("class", "btn btn-danger");
                        break;
                    case Button.Inverse:
                        htmlAttributes.Add("class", "btn btn-inverse");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public static class ActionLinkExtensions
    {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName)
        {
            return ActionLink(htmlHelper, appearance, actionName, null, null, null);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, object routeValues)
        {
            return ActionLink(htmlHelper, appearance, actionName, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, RouteValueDictionary routeValues)
        {
            return ActionLink(htmlHelper, appearance, actionName, null, routeValues, null);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, string controllerName)
        {
            return ActionLink(htmlHelper, appearance, actionName, controllerName, null, null);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, object routeValues, object htmlAttributes)
        {
            return ActionLink(htmlHelper, appearance, actionName, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            throw new NotImplementedException();
        }
        
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var innerHtml = appearance.Caption;
            //if (!String.IsNullOrWhiteSpace(buttonIcon))
            //{
            //    var i = new TagBuilder("i");
            //    i.AddCssClass(buttonIcon);
            //    innerHtml = i.ToString(TagRenderMode.Normal);

            //    if (!String.IsNullOrWhiteSpace(caption))
            //        innerHtml += @"&nbsp;" + caption;
            //}

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var a = new TagBuilder("a");
            a.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            a.MergeAttributes(htmlAttributes);
            a.InnerHtml = innerHtml;
            if (appearance.Button != Button.None)
            {
                a.AddCssClass("btn");
                switch (appearance.Button)
                {
                    case Button.None:
                        break;
                    case Button.Normal:
                        break;
                    case Button.Primary:
                        a.AddCssClass("btn-primary");
                        break;
                    case Button.Info:
                        a.AddCssClass("btn-info");
                        break;
                    case Button.Success:
                        a.AddCssClass("btn-success");
                        break;
                    case Button.Warning:
                        a.AddCssClass("btn-warning");
                        break;
                    case Button.Danger:
                        a.AddCssClass("btn-danger");
                        break;
                    case Button.Inverse:
                        a.AddCssClass("btn-inverse");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }
        
        //public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        //{
        //    throw new NotImplementedException();
        //}
        
        //public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, Appearance appearance, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        //{
        //    throw new NotImplementedException();
        //}
    }
}