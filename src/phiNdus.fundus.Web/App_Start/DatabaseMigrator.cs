using System;
using System.Configuration;
using System.IO;
using phiNdus.fundus.DbMigrations;

namespace phiNdus.fundus.Web.App_Start
{
    public class DatabaseMigrator
    {
        public static void Migrate(Func<string, string> mapPath)
        {
            using (var writer = new StreamWriter(mapPath(@"~\App_Data\Logs\DbMigration.log"), true))
            {
                var connectionString = ConfigurationManager.ConnectionStrings["phundus"].ConnectionString;
                var tags = ConfigurationManager.AppSettings["MigrationTags"].Split(',');
                var profile = ConfigurationManager.AppSettings["MigrationProfile"];

                Runner.MigrateToLatest(
                    connectionString,
                    writer,
                    tags,
                    profile);
            }
        }
    }
}