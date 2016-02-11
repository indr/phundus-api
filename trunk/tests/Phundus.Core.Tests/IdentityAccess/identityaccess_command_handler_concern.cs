namespace Phundus.Tests.IdentityAccess
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Queries;

    public class identityaccess_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;
        protected static IOrganizationRepository organizationRepository;

        protected static identityaccess_factory make;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            organizationRepository = depends.on<IOrganizationRepository>();
            make = new identityaccess_factory(fake);
        };
    }
}