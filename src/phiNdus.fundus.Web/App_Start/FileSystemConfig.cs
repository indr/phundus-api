namespace phiNdus.fundus.Web.App_Start
{
    using System.IO;
    using System.Web.Hosting;

    public class FileSystemConfig
    {
        public static void CreateMissingDirectory()
        {
            Directory.CreateDirectory(HostingEnvironment.MapPath(@"~\App_Data\Logs"));
            Directory.CreateDirectory(HostingEnvironment.MapPath(@"~\Content\Images\Articles"));
        }
    }
}