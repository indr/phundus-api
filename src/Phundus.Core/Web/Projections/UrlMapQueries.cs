namespace Phundus.Web.Projections
{
    using Common.Querying;

    public interface IUrlMapQueries
    {
        UrlMapData FindByUrl(string url);
    }

    public class UrlMapQueries : QueryBase<UrlMapData>, IUrlMapQueries
    {
        public UrlMapData FindByUrl(string url)
        {
            return SingleOrDefault(p => p.Url == url);
        }
    }
}