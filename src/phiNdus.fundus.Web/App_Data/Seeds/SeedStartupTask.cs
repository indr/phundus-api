namespace Phundus.Web.App_Data.Seeds
{
    using System;
    using System.Configuration;
    using Bootstrap.Extensions.StartupTasks;
    using Common;
    using Common.Cqrs;
    using Core.Cqrs;

    public abstract class SeedStartupTask : IStartupTask
    {
        private readonly ICommandDispatcher _dispatcher;

        protected SeedStartupTask(ICommandDispatcher commandDispatcher)
        {
            AssertionConcern.AssertArgumentNotNull(commandDispatcher, "Command dispatcher must be provided.");

            _dispatcher = commandDispatcher;
        }

        public string MigrationProfile { get; private set; }

        public string[] MigrationTags { get; private set; }

        public void Run()
        {
            MigrationTags = ConfigurationManager.AppSettings["MigrationTags"].Split(',');
            MigrationProfile = ConfigurationManager.AppSettings["MigrationProfile"];

            if (String.IsNullOrWhiteSpace(MigrationProfile))
                return;

            if (MigrationProfile != "Acceptance")
                return;

            Seed();
        }

        public void Reset()
        {
            // No-op
        }

        protected void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            _dispatcher.Dispatch(command);
        }

        protected abstract void Seed();
    }
}