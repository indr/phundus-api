using System;
using System.IO;

namespace phiNdus.fundus.Web.App_Start
{
    public class FileSystemConfig
    {
        public static void CreateMissingDirectory(Func<string , string> mapPath)
        {
            Directory.CreateDirectory(mapPath(@"~\App_Data\Logs"));
            Directory.CreateDirectory(mapPath(@"~\Content\Images\Articles"));
        }
    }
}