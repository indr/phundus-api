namespace Phundus.Tests
{
    using System;
    using System.Linq.Expressions;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
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
            initiatorService.setup(x => x.GetById(theInitiatorId)).Return(theInitiator);
        };

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            if (catchException)
                caughtException = Catch.Exception(() => sut.Handle(command));
            else
                sut.Handle(command);
        };

        protected static IMethodCallOccurrence enforceInitiatorTo<TAccessObject>() where TAccessObject : IAccessObject
        {
            return authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<TAccessObject>.Is.NotNull));
        }
        protected static IMethodCallOccurrence enforceInitiatorTo<TAccessObject>(Expression<Predicate<TAccessObject>> accessObjectPredicate) where TAccessObject : IAccessObject
        {
            return authorize.WasToldTo(x =>
                x.Enforce(Arg<InitiatorId>.Is.Equal(theInitiatorId),
                    Arg<TAccessObject>.Matches(accessObjectPredicate)));
        }
    }
}