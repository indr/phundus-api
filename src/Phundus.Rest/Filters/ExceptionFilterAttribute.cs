namespace Phundus.Rest.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Filters;
    using Common;

    /// <summary>
    /// http://weblogs.asp.net/cibrax/handling-exceptions-in-your-asp-net-web-api
    /// </summary>
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, HttpStatusCode> _mappings = new Dictionary<Type, HttpStatusCode>();

        public ExceptionFilterAttribute()
        {
            _mappings.Add(typeof(ArgumentNullException), HttpStatusCode.BadRequest);
            _mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
            _mappings.Add(typeof(AuthorizationException), HttpStatusCode.Forbidden);
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            if ((context == null) || (context.Exception == null))
                return;

            Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);

            var exception = context.Exception;
            var statusCode = GetStatusCode(exception);

            context.Response = context.Request.CreateErrorResponse(statusCode, exception.Message);
        }

        private HttpStatusCode GetStatusCode(Exception exception)
        {
            if (exception is HttpException)
            {
                return (HttpStatusCode)(exception as HttpException).GetHttpCode();
            }
            
            if (_mappings.ContainsKey(exception.GetType()))
            {
               return _mappings[exception.GetType()];
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}