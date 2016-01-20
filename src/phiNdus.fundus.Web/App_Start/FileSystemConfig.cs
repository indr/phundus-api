namespace phiNdus.fundus.Web.App_Start
{
    using System.IO;
    using System.Web;

    public class FileSystemConfig
    {
        public static void CreateMissingDirectory()
        {
            var directories = new[]
            {
                @"~\App_Data\Logs",
                @"~\App_Data\Mails",
                @"~\Content\Images\Articles",
                @"~\Content\Uploads\Organizations"
            };

            foreach (var directory in directories)
                CreateDirectory(directory);
        }

        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
        }
    }
}