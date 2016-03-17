namespace Phundus.Web.Projections
{
    using Common.Querying;

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