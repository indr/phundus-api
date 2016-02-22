namespace Phundus.Tests.IdentityAccess
{
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
        protected static IMemberInRole memberInRole;
        private static IUserInRole userInRole;

        protected static IOrganizationRepository organizationRepository;
        protected static IUserRepository userRepository;

        protected static identityaccess_factory make;

        protected static Admin theAdmin;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);

            theAdmin = make.Admin();
            userInRole = depends.on<IUserInRole>();
            userInRole.WhenToldTo(x => x.Admin(theInitiatorId)).Return(theAdmin);

            memberInRole = depends.on<IMemberInRole>();
            organizationRepository = depends.on<IOrganizationRepository>();
            userRepository = depends.on<IUserRepository>();
        };
    }
}