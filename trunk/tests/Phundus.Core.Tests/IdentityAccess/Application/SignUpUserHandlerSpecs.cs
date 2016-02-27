namespace Phundus.Tests.IdentityAccess.Application
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
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
            command = new SignUpUser(new UserId(), "MAIL@DOMAIN.COM", "Password", "FirstName", "LastName", "Street", "Postcode",
                "City", "MobilePhone");
        };

        public It should_add_new_user_to_repository =
            () => userRepository.WasToldTo(x => x.Add(Arg<User>.Is.NotNull));

        public It should_ask_for_unique_email_address =
            () => userRepository.WasToldTo(x => x.FindByEmailAddress(command.EmailAddress.ToLowerInvariant().Trim()));

        public It should_publish_user_signed_up = () =>
            Published<UserSignedUp>(p => p.City == "City"
                                         && p.EmailAddress == "mail@domain.com"
                                         && p.FirstName == "FirstName"
                                         && p.JsNumber == null
                                         && p.LastName == "LastName"
                                         && p.MobilePhone == "MobilePhone"
                                         && p.Password != "Password"
                                         && p.Postcode == "Postcode"
                                         && p.Salt != ""
                                         && p.Street == "Street"
                                         && p.UserGuid == command.UserId.Id);
    }
}