namespace Phundus.Web.Application
{
    using System;
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

    public class UrlMapData
    {
        public virtual Guid RowId { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid? OrganizationId { get; set; }
        public virtual Guid? UserId { get; set; }
    }
}