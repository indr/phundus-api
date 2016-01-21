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

        protected static bool testResult;

        private Because of = () =>
        {
            theAccessObject.ShouldNotBeNull();
            testResult = sut.Test(theUserId, theAccessObject);
            if (catchException)
                caughtException = Catch.Exception(() => sut.Enforce(theUserId, theAccessObject));
            else
                sut.Enforce(theUserId, theAccessObject);
        };
    }
}