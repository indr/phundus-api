namespace Phundus.Tests
{
    using System;
    using System.Net.Mail;
    using Common.Domain.Model;
    using Common.Mailing;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class mail_concern<T> : Observes<T> where T : class
    {
        protected static IMailGateway gateway;
        protected static IMessageFactory factory;

        protected static Initiator initiator = new Initiator(new InitiatorId(), "initiator@test.phundus.ch", "The Initiator");

        protected static MailMessage message;

        private Establish ctx = () =>
        {
            gateway = depends.on<IMailGateway>();
            factory = depends.on<IMessageFactory>(new MessageFactory());

            message = null;
            gateway.setup(x => x.Send(Arg<DateTime>.Is.Anything, Arg<MailMessage>.Is.Anything))
                .Callback((Action<DateTime, MailMessage>)((d, m) => message = m));
        };
    }
}