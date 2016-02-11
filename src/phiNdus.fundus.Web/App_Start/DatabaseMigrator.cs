namespace Phundus.Web
{
    using System.Configuration;
    using System.IO;
    using System.Web.Hosting;
    using Phundus.Migrations;

    public class DatabaseMigrator
    {
        public static void Migrate()
        {
            using (var writer = new StreamWriter(HostingEnvironment.MapPath(@"~\App_Data\Logs\DbMigration.log"), true))
            {
                var connectionString = ConfigurationManager.ConnectionStrings["phundus"];
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