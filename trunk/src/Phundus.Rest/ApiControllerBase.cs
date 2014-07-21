namespace Phundus.Rest
{
    using System.Security.Principal;
    using System.Web.Http;
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

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}