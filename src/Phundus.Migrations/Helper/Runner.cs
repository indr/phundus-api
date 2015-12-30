namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using FluentMigrator;
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Announcers;
    using FluentMigrator.Runner.Initialization;
    using FluentMigrator.Runner.Processors;
    using FluentMigrator.Runner.Processors.SqlServer;

    public static class Runner
    {
        public static void MigrateToLatest(ConnectionStringSettings connectionString, TextWriter writer, IEnumerable<string> tags, string profile)
        {
            writer.WriteLine();
            writer.WriteLine("/* ========== Start Migration " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " */\n");

            try
            {
                var announcer = new TextWriterAnnouncer(writer)
                                    {
                                        ShowElapsedTime = true,
                                        ShowSql = true
                                    };

                var assembly = Assembly.GetAssembly(typeof (M0_EmptyDatabase));

                var migrationContext = new RunnerContext(announcer)
                                           {
                                               ApplicationContext = connectionString.ProviderName,
                                               Namespace = "phiNdus.fundus.DbMigrations",
                                               Tags = tags,
                                               Profile = profile
                                           };
                
                IMigrationProcessorOptions options = new ProcessorOptions
                                                         {
                                                             PreviewOnly = false,
                                                             Timeout = 60                                                             
                                                         };
                
                MigrationProcessorFactory factory;
                if (connectionString.ProviderName == "System.Data.SqlServerCe.4.0")
                {
                    factory = new SqlServerCeProcessorFactory();
                }
                else
                {
                    factory = new SqlServer2008ProcessorFactory();
                }

                var processor = factory.Create(connectionString.ConnectionString, announcer, options);

                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp();
            }
            catch(Exception ex)
            {
                writer.WriteLine("/* Exception: {0} */\n", ex.ToString());
                throw;
            }
            finally
            {
                writer.WriteLine("\n/* ========== End Migration " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " */\n");
            }
        }
    }
}