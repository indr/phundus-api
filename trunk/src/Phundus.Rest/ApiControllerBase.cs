namespace Phundus.Rest
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Security;
    using AutoMapper;
    using Castle.Core.Logging;
    using Common.Cqrs;
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

        protected void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            Dispatcher.Dispatch(command);
        }

        protected int CurrentUserId
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    throw new AuthenticationException();
                var userId = user.ProviderUserKey;
                return Convert.ToInt32(userId);
            }
        }

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
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
    }
}