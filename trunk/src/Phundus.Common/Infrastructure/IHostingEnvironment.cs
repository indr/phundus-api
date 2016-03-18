namespace Phundus.Common.Infrastructure
{
    using System.Web.Hosting;

    public interface IHostingEnvironment
    {
        string MapPath(string virtualPath);
    }

    public class HostingEnvironmentImpl : IHostingEnvironment
    {
        public string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
    }
}