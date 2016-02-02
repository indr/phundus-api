namespace Phundus.Tests
{
    using System;
    using System.Linq.Expressions;
    using Authorization;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Rhino.Mocks;

    public abstract class command_handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;
        protected static TCommand command;

        protected static IAuthorize authorize;
        protected static IInitiatorService initiatorService;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");

            authorize = depends.on<IAuthorize>();
            initiatorService = depends.on<IInitiatorService>();
            initiatorService.WhenToldTo(x => x.GetActiveById(theInitiatorId)).Return(theInitiator);
        };

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            if (catchException)
                caughtException = Catch.Exception(() => sut.Handle(command));
            else
                sut.Handle(command);
        };

        protected static IMethodCallOccurrence EnforcedInitiatorTo<T>(Expression<Predicate<T>> accessObjectPredicate)
        {
            return authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<T>.Matches(accessObjectPredicate)));
        }
    }
}