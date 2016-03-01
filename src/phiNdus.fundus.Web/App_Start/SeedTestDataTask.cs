namespace Phundus.Web
{
    using System;
    using System.Configuration;
    using Bootstrap.Extensions.StartupTasks;
    using Castle.Core.Logging;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Organizations.Model;

    public class SeedTestDataTask : IStartupTask
    {
        private readonly ILogger _logger;
        private readonly ICommandDispatcher _dispatcher;

        public SeedTestDataTask(ILogger logger, ICommandDispatcher dispatcher)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [Transaction]
        public virtual void Run()
        {
            _logger.Info("Running task SeedTestDataTask.");
            if (!IsResetDatabaseProfileSpecified())
            {
                _logger.Info("Skipping task because ResetDatabase profile is not specified.");
                return;
            }

            var adminId = new UserId();
            var managerId = new UserId();
            var memberId = new UserId();
            var organizationId = new OrganizationId();
            MembershipApplicationId applicationId = null;

            Dispatch(new SignUpUser(adminId, "admin@test.phundus.ch", "1234", "John", "Root", "", "", "", ""));            
            Dispatch(new EstablishOrganization(new InitiatorId(adminId), organizationId, "Scouts"));

            applicationId = new MembershipApplicationId();
            Dispatch(new SignUpUser(managerId, "manager@test.phundus.ch", "1234", "Greg", "Manager", "", "", "", ""));
            Dispatch(new ApproveUser(new InitiatorId(adminId), managerId));
            Dispatch(new ApplyForMembership(new InitiatorId(managerId), applicationId, managerId, organizationId));
            Dispatch(new ApproveMembershipApplication(new InitiatorId(adminId), applicationId));
            Dispatch(new ChangeMembersRole(new InitiatorId(adminId), organizationId, managerId, MemberRole.Manager));

            applicationId = new MembershipApplicationId();
            Dispatch(new SignUpUser(memberId, "member@test.phundus.ch", "1234", "Alice", "Member", "", "", "", ""));
            Dispatch(new ApproveUser(new InitiatorId(adminId), memberId));
            Dispatch(new ApplyForMembership(new InitiatorId(memberId), applicationId, memberId, organizationId));
            Dispatch(new ApproveMembershipApplication(new InitiatorId(managerId), applicationId));
        }

        private bool IsResetDatabaseProfileSpecified()
        {
            return ConfigurationManager.AppSettings["MigrationProfile"] == "ResetDatabase";
        }

        private void Dispatch<TCommand>(TCommand command)
        {
            _dispatcher.Dispatch(command);
        }

        public virtual void Reset()
        {
        }
    }
}