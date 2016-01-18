namespace Phundus.Tests.IdentityAccess
{
    using Machine.Specifications;
    using Phundus.Cqrs;

    public class identityaccess_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
    {
        private Establish ctx = () =>
            make = new identityaccess_factory(fake);

        protected static identityaccess_factory make;
    }
}