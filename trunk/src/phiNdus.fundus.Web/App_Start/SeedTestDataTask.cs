namespace Phundus.Web
{
    using System;
    using System.Configuration;
    using System.Threading;
    using Bootstrap.Extensions.StartupTasks;
    using Castle.Core.Logging;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Organizations.Model;
    using Inventory.Application;

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

        public virtual void Run()
        {
            _logger.Info("Running task SeedTestDataTask.");
            if (!IsResetDatabaseProfileSpecified())
            {
                _logger.Info("Skipping task because ResetDatabase profile is not specified.");
                return;
            }

            
            var adminId = new InitiatorId();
            var managerId = new InitiatorId();
            var memberId = new InitiatorId();
            var organizationId = new OrganizationId();
            MembershipApplicationId applicationId = null;

            Dispatch(new SignUpUser(adminId, "admin@test.phundus.ch", "1234", "John", "Root", "", "", "", ""));
            Dispatch(new EstablishOrganization(adminId, organizationId, "Scouts"));
           
            applicationId = new MembershipApplicationId();
            Dispatch(new SignUpUser(managerId, "manager@test.phundus.ch", "1234", "Greg", "Manager", "", "", "", ""));
            Dispatch(new ApproveUser(adminId, managerId));
            Dispatch(new ApplyForMembership(managerId, applicationId, managerId, organizationId));            
            Dispatch(new ApproveMembershipApplication(adminId, applicationId));
            Dispatch(new ChangeMembersRole(adminId, organizationId, managerId, MemberRole.Manager));

            applicationId = new MembershipApplicationId();
            Dispatch(new SignUpUser(memberId, "member@test.phundus.ch", "1234", "Alice", "Member", "", "", "", ""));
            Dispatch(new ApproveUser(adminId, memberId));
            Dispatch(new ApplyForMembership(memberId, applicationId, memberId, organizationId));            
            Dispatch(new ApproveMembershipApplication(managerId, applicationId));

            Thread.Sleep(1000);
            Dispatch(new OpenStore(adminId, new OwnerId(organizationId.Id), new StoreId()));
        }

        private static bool IsResetDatabaseProfileSpecified()
        {
            return ConfigurationManager.AppSettings["MigrationProfile"] == "ResetDatabase";
        }

        private void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            _dispatcher.Dispatch(command);
            Thread.Sleep(300);
        }

        public virtual void Reset()
        {
        }
    }
}