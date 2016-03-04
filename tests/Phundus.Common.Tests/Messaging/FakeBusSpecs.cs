namespace Phundus.Common.Tests.Messaging
{
    using System;
    using Castle.Core.Logging;
    using Commanding;
    using Common.Messaging;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public abstract class fake_bus_concern : Observes<FakeBus>
    {
        protected static ILogger logger;
        protected static ICommandDispatcher dispatcher;

        private Establish ctx = () =>
        {
            logger = depends.on<ILogger>();
            dispatcher = depends.on<ICommandDispatcher>();
        };
    }

    //[Subject(typeof (FakeBus))]
    //public class when_sending_command_that_throws_an_error : fake_bus_concern
    //{
    //    private static ICommand command;

    //    private Establish ctx = () =>
    //    {
    //        command = fake.an<ICommand>();
    //        dispatcher.setup(x => x.Dispatch(command)).Throw(new ApplicationException());
    //    };

    //    private Because of = () =>
    //        sut.Send(command);

    //    private It should_log_fatal = () =>
    //        logger.received(x => x.Fatal(Arg<string>.Is.Anything, Arg<Exception>.Is.NotNull));
    //}
}