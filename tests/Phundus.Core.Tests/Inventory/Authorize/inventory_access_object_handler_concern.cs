namespace Phundus.Tests.Inventory.Authorize
{
    using Authorization;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Projections;

    public class inventory_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static inventory_factory make;
        protected static IMemberInRole memberInRole;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            make = new inventory_factory(fake);
        };
    }
}