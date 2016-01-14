namespace Phundus.Core.Tests
{
    using Common.Domain.Model;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Users.Model;
    using Machine.Specifications;
    using NUnit.Framework;

    public abstract class handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static TCommand command;

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            sut.Handle(command);
        };

        protected static User CreateAdmin(int userId)
        {
            return CreateAdmin(new UserId(userId));
        }

        protected static User CreateAdmin(UserId userId)
        {
            var result = new User("admin@test.phundus.ch", "1234", "Bob", "Cooper", "Street", "1000", "City",
                "012 345 67 89", null);
            var property = typeof (User).GetProperty("Id");
            Assert.That(property, Is.Not.Null, "Could not find property Id of type {0}. " +
                                               "If User is no longer of base type EntityBase and has no database generated numeric identifier, remove this hack.",
                typeof (User));
            property.GetSetMethod(true).Invoke(result, new object[] {userId.Id});

            return result;
        }
    }
}