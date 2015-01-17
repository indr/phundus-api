namespace Phundus.Core.Migrations
{
    using Bootstrap.Extensions.StartupTasks;
    using Cqrs;

    public abstract class DomainModelMigrationStartupTaskBase : IStartupTask
    {
        private readonly ICommandDispatcher _commandDispatcher;

        protected DomainModelMigrationStartupTaskBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public abstract void Run();

        public void Reset()
        {
        }

        protected void Dispatch<TCommand>(TCommand command)
        {
            _commandDispatcher.Dispatch(command);
        }
    }
}