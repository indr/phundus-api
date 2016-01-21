namespace Phundus.Tests
{
    using Authorization;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class access_object_handler_concern<TAccessObject, TAccessObjectHandler> : Observes<TAccessObjectHandler>
        where TAccessObjectHandler : class, IHandleAccessObject<TAccessObject>
    {
        protected static UserId theUserId;
        protected static TAccessObject theAccessObject;

        private Because of = () =>
        {
            theAccessObject.ShouldNotBeNull();
            sut.Handle(theUserId, theAccessObject);
        };
    }
}