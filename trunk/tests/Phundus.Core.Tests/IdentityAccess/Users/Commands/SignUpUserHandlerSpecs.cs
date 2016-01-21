namespace Phundus.Tests.IdentityAccess.Users.Commands
{
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Commands;
    using Phundus.IdentityAccess.Users.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (SignUpUserHandler))]
    public class when_register_user_is_handled : command_handler_concern<SignUpUser, SignUpUserHandler>
    {
        private static IUserRepository userRepository;

        public Establish c = () =>
        {
            userRepository = depends.on<IUserRepository>();
            command = new SignUpUser("MAIL@DOMAIN.COM", "Password", "FirstName", "LastName", "Street", "Postcode",
                "City", "MobilePhone");
        };

        public It should_add_new_user_to_repository =
            () => userRepository.WasToldTo(x => x.Add(Arg<User>.Is.NotNull));

        public It should_ask_for_unique_email_address =
            () => userRepository.WasToldTo(x => x.FindByEmailAddress(command.EmailAddress.ToLowerInvariant().Trim()));

        public It should_publish_user_registered =
            () => publisher.WasToldTo(x => x.Publish(Arg<UserSignedUp>.Is.NotNull));
    }
}