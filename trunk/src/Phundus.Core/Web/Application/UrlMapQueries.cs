namespace Phundus.Web.Application
{
    using Common.Querying;
    using Projections;

    public interface IUrlMapQueryService
    {
        UrlMapData FindByUrl(string url);
    }

    public class UrlMapQueryService : QueryServiceBase<UrlMapData>, IUrlMapQueryService
    {
        public UrlMapData FindByUrl(string url)
        {
            return SingleOrDefault(p => p.Url == url);
        }
    }
}