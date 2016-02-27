namespace Phundus.Tests.Shop.Authorization
{
    using Machine.Specifications;
    using Phundus.Authorization;
    using Phundus.IdentityAccess.Projections;
    using Phundus.Shop.Model;

    public class shop_access_object_handler_concern<TAccessObject, TAccessObjectHandler> :
        access_object_handler_concern<TAccessObject, TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static shop_factory make;
        protected static IMemberInRole memberInRole;
        protected static ILessorService lessorService;

        private Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            lessorService = depends.on<ILessorService>();
            make = new shop_factory(fake);
        };
    }
}