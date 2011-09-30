using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Helpers
{
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

        public static MvcContainer BeginContainerFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return new MvcContainer(htmlHelper.ViewContext, id);
        }
    }
}
