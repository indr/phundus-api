using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace phiNdus.fundus.Web.Helpers
{
    public static class HtmlExtensions
    {

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


        // http://stackoverflow.com/questions/3065307/client-id-for-property-asp-net-mvc
        //public static MvcHtmlString IdFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
        //                                                  Expression<Func<TModel, TValue>> expression)
        //{
        //    var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        //    var inputFieldId = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
        //    return MvcHtmlString.Create(inputFieldId);
        //}

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