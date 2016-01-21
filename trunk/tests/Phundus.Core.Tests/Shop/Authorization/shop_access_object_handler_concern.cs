namespace Phundus.Tests.Shop.Authorization
{
    using Machine.Specifications;
    using Phundus.Authorization;
    using Phundus.IdentityAccess.Queries;

    public class shop_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static shop_factory make;
        protected static IMemberInRole memberInRole;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            make = new shop_factory(fake);
        };
    }
}