namespace phiNdus.fundus.DbMigrations
{
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
        public static void MigrateToLatest(string connectionString, TextWriter writer)
        {
            var announcer = new TextWriterAnnouncer(writer);
            var assembly = Assembly.GetAssembly(typeof (M0_EmptyDatabase));

            var migrationContext = new RunnerContext(announcer)
                                       {
                                           Namespace = "phiNdus.fundus.DbMigrations"
                                       };
            IMigrationProcessorOptions options = new ProcessorOptions
                                                     {
                                                         PreviewOnly = false,
                                                         Timeout = 60
                                                     };
            var factory = new SqlServer2008ProcessorFactory();
            var processor = factory.Create(connectionString, announcer, options);

            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp();
        }
    }
}