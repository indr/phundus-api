using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.Helpers
{
    //public class HandleAjaxErrorAttribute : HandleErrorAttribute
    //{
    //    public override void OnException(ExceptionContext filterContext)
    //    {
    //        if (filterContext == null)
    //        {
    //            throw new ArgumentNullException("filterContext");
    //        }

    //        if (filterContext.IsChildAction)
    //        {
    //            return;
    //        }

    //        if (filterContext.ExceptionHandled)
    //        {
    //            return;
    //        }

    //        Exception exception = filterContext.Exception;

    //        if (!ExceptionType.IsInstanceOfType(exception))
    //        {
    //            return;
    //        }

    //        //***** This is the new code  *****//
    //        if (filterContext.HttpContext.Request.IsAjaxRequest()) // If it's a ajax request
    //        {
    //            filterContext.Result = new ContentResult
    //                                       {
    //                                           Content = exception.Message,
    //                                           ContentEncoding = Encoding.UTF8,
    //                                           ContentType = "text/plain"
    //                                       };
    //            filterContext.ExceptionHandled = true;
    //            filterContext.HttpContext.Response.Clear();
    //            filterContext.HttpContext.Response.StatusCode = 500;  // Maybe it should be 500, but this way you handle the JQuery on the success event
    //            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
    //        }
    //        else 
    //        {   
    //           base.OnException(filterContext);
    //        }
    //    }

    //}
}