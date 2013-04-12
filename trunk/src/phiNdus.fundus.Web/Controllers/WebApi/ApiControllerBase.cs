namespace phiNdus.fundus.Web.Controllers.WebApi
{
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

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}