namespace Phundus.Web
{
    using System;
    using System.Configuration;
    using Bootstrap.Extensions.StartupTasks;
    using Castle.Core.Logging;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Users.Commands;

    public class SignUpRootTask : IStartupTask
    {
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _dispatcher;

        public SignUpRootTask(ILogger logger, ICommandDispatcher dispatcher)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [Transaction]
        public virtual void Run()
        {
            _logger.Info("Running task SignUpRootTask.");
            if (!IsResetDatabaseProfileSpecified())
            {
                _logger.Info("Skipping task because ResetDatabase profile is not specified.");
                return;
            }

            _dispatcher.Dispatch(new SignUpUser(new UserId(), "admin@test.phundus.ch", "1234", "", "Root", "", "", "", ""));
        }

        private bool IsResetDatabaseProfileSpecified()
        {
            return ConfigurationManager.AppSettings["MigrationProfile"] == "ResetDatabase";
        }

        public virtual void Reset()
        {
        }
    }
}