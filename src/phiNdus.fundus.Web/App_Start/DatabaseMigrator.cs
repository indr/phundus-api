namespace Phundus.Web
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web.Hosting;
    using Migrations;

    public class DatabaseMigrator
    {
        public static void Migrate()
        {
            var logFile = HostingEnvironment.MapPath(@"~\App_Data\Logs\DbMigration.log");
            if (logFile == null)
                throw new Exception(@"Could not map ~\App_Data\Logs\DbMigration.log.");

            using (var writer = new StreamWriter(logFile, true))
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