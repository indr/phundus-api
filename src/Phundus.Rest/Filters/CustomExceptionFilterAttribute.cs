namespace Phundus.Rest.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context == null)
                return;
            
            Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);

            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                context.Exception.Message);
        }
    }
}