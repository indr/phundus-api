namespace Phundus.Tests
{
    using Authorization;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class access_object_handler_concern<TAccessObject, TAccessObjectHandler> : concern<TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static UserId theUserId;
        protected static TAccessObject theAccessObject;

        private Because of = () =>
        {
            theAccessObject.ShouldNotBeNull();
            if (catchException)
                caughtException = Catch.Exception(() => sut.Handle(theUserId, theAccessObject));
            else
                sut.Handle(theUserId, theAccessObject);
        };
    }
}