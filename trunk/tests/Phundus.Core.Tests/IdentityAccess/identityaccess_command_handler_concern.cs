namespace Phundus.Tests.IdentityAccess
{
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Organizations;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Resources;

    public class identityaccess_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
        where TCommand : ICommand
    {
        protected static identityaccess_factory make;

        protected static IMemberInRole memberInRole;
        protected static IUserInRole userInRole;

        protected static IOrganizationRepository organizationRepository;
        protected static IUserRepository userRepository;

        protected static Admin theAdmin;
        protected static Manager theManager;
        protected static OrganizationId theOrganizationId;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);

            theOrganizationId = new OrganizationId();

            userInRole = depends.on<IUserInRole>();

            userInRole.setup(x => x.Initiator(theInitiatorId)).Return(theInitiator);

            theAdmin = make.Admin();
            userInRole.setup(x => x.Admin(theInitiatorId)).Return(theAdmin);

            theManager = new Manager(theInitiatorId, "manager@test.phundus.ch", "The Manager");
            userInRole.setup(x => x.Manager(theInitiatorId, theOrganizationId)).Return(theManager);

            memberInRole = depends.on<IMemberInRole>();
            organizationRepository = depends.on<IOrganizationRepository>();
            userRepository = depends.on<IUserRepository>();
        };
    }
}