namespace Phundus.Web.Projections
{
    using Common.Querying;

    public interface IUrlMapQueries
    {
        UrlMapData FindByUrl(string url);
    }

    public class UrlMapQueries : QueryServiceBase<UrlMapData>, IUrlMapQueries
    {
        public UrlMapData FindByUrl(string url)
        {
            return SingleOrDefault(p => p.Url == url);
        }
    }
}