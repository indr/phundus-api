namespace Phundus.Core.Tests.IdentityAndAccess.Users.Commands
{
    using Core.IdentityAndAccess.Users.Commands;
    using Core.IdentityAndAccess.Users.Model;
    using Core.IdentityAndAccess.Users.Repositories;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (RegisterUserHandler))]
    public class when_register_user_is_handled : handler_concern<RegisterUser, RegisterUserHandler>
    {
        private static IUserRepository repository;
        private static int userId = 99;

        public Establish c = () =>
        {
            repository = depends.on<IUserRepository>();
            repository.setup(x => x.Add(Arg<User>.Is.NotNull)).Return(userId);


            command = new RegisterUser("MAIL@DOMAIN.COM", "Password", "FirstName", "LastName", "Street", "Postcode",
                "City", "MobilePhone", 123456);
        };

        public It should_add_new_user_to_repository =
            () => repository.WasToldTo(x => x.Add(Arg<User>.Is.NotNull));

        public It should_ask_for_unique_email_address =
            () => repository.WasToldTo(x => x.FindByEmail(command.EmailAddress.ToLowerInvariant().Trim()));

        public It should_publish_user_registered =
            () => publisher.WasToldTo(x => x.Publish(Arg<UserRegistered>.Is.NotNull));

        public It should_set_user_id_on_command =
            () => command.UserId.ShouldEqual(userId);
    }
}