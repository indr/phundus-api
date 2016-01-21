namespace Phundus.Tests
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;

    public class domain_event_concern<T> : serialization_object_concern<T> where T : class
    {
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");
        };
    }
}