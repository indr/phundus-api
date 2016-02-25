namespace Phundus.Tests.IdentityAccess.Authorize
{
    using Authorization;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Projections;
    using Phundus.IdentityAccess.Users.Services;

    public class identityaccess_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static identityaccess_factory make;
        protected static IMemberInRole memberInRole;
        protected static IUserInRole userInRole;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            userInRole = depends.on<IUserInRole>();
            make = new identityaccess_factory(fake);
        };
    }
}