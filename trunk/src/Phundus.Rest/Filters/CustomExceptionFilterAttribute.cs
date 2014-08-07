namespace Phundus.Rest.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                context.Exception.Message);
        }
    }
}