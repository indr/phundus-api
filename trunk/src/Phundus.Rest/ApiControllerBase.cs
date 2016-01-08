namespace Phundus.Rest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Security;
    using AutoMapper;
    using Castle.Core.Logging;
    using Common.Domain.Model;
    using Core.Cqrs;

    public class ApiControllerBase : ApiController
    {
        private ILogger _log = NullLogger.Instance;

        public ILogger Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public IIdentity Identity { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        protected CurrentUserId CurrentUserId
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    throw new AuthenticationException();
                var userId = Convert.ToInt32(user.ProviderUserKey);
                return new CurrentUserId(userId);
            }
        }

        protected void Dispatch<TCommand>(TCommand command)
        {
            Dispatcher.Dispatch(command);
        }

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        protected HttpResponseMessage NoContent()
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        protected HttpResponseMessage CreateNotFoundResponse(string format, object arg0)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(format, arg0));
        }

        protected HttpResponseMessage CreatePdfResponse(Stream stream, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = string.Format(fileName)
            };

            return result;
        }

        protected Dictionary<string, string> GetQueryParams()
        {
            return Request.GetQueryNameValuePairs().ToDictionary(ks => ks.Key, es => es.Value);
        }
    }
}