namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Security.Principal;
    using System.Web.Http;
    using AutoMapper;
    using Castle.Core.Logging;

    public class ApiControllerBase : ApiController
    {
        private ILogger _log = NullLogger.Instance;

        public ILogger Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public IIdentity Identity { get; set; }

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}