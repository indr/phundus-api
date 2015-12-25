namespace Phundus.Rest.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Filters;
    using Common;

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context == null)
                return;

            Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);

            if (context.Exception is NotImplementedException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotImplemented,
                    context.Exception.Message);
            }
            else if (context.Exception is AuthorizationException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                    context.Exception.Message);
            }
            else if (context.Exception is HttpException)
            {
                var statusCode = (HttpStatusCode) (context.Exception as HttpException).GetHttpCode();
                context.Response = context.Request.CreateErrorResponse(statusCode, context.Exception.Message);
            }
            else
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    context.Exception.Message);
            }
        }
    }
}