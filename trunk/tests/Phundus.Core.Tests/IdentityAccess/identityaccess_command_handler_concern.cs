namespace Phundus.Tests.IdentityAccess
{
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Queries;
    using Phundus.IdentityAccess.Users.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Phundus.IdentityAccess.Users.Services;

    public class identityaccess_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
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
            theAdmin = make.Admin();
            userInRole.WhenToldTo(x => x.Admin(theInitiatorId)).Return(theAdmin);
            theManager = make.Manager();
            userInRole.WhenToldTo(x => x.Manager(theInitiatorId, theOrganizationId)).Return(theManager);

            memberInRole = depends.on<IMemberInRole>();
            organizationRepository = depends.on<IOrganizationRepository>();
            userRepository = depends.on<IUserRepository>();
        };
    }
}