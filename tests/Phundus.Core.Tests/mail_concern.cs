namespace Phundus.Tests
{
    using Common.Domain.Model;
    using Common.Mailing;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class mail_concern<T> : Observes<T> where T : class
    {
        protected static IMailGateway gateway;
        protected static IMessageFactory factory;

        protected static Initiator initiator = new Initiator(new InitiatorId(), "initiator@test.phundus.ch", "The Initiator");

        private Establish ctx = () =>
        {
            gateway = depends.on<IMailGateway>();
            factory = depends.on<IMessageFactory>(new MessageFactory(new ModelFactory()));
        };
    }
}