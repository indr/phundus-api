namespace Phundus.Rest
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Security;
    using AutoMapper;
    using Castle.Core.Logging;
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

        protected void Dispatch<TCommand>(TCommand command)
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
    }
}