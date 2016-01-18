namespace Phundus.Tests
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using NUnit.Framework;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Users.Model;

    public abstract class handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static InitiatorId theInitiatorId;
        protected static TCommand command;

        private Establish ctx = () => theInitiatorId = new InitiatorId();

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            if (catchException)
                caughtException = Catch.Exception(() => sut.Handle(command));
            else
                sut.Handle(command);
        };

        protected static Exception caughtException;
        protected static bool catchException = false;

        protected static User CreateAdmin(Guid userId)
        {
            return CreateAdmin(new UserId(userId));
        }

        protected static User CreateAdmin(UserId userId)
        {
            return CreateUser(userId, "admin@test.phundus.ch");
        }

        protected static User CreateUser(UserId userId, string emailAddress = "user@test.phundus.ch")
        {
            var argumentsForConstructor = new Object[]
            {emailAddress, "1234", "Bob", "Cooper", "Street", "1000", "City", "012 345 67 89", null};
            var result = mock.partial<User>(argumentsForConstructor);

            var property = typeof (User).GetProperty("Guid");
            Assert.That(property, Is.Not.Null, "Could not find property Id of type {0}. " +
                                               "If User is no longer of base type EntityBase and has no database generated numeric identifier, remove this hack.",
                typeof (User));
            property.GetSetMethod(true).Invoke(result, new object[] {userId.Id});

            return result;
        }
    }
}