namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Model.Users;
    using Users.Exceptions;
    using Users.Model;

    public class SignUpUser
    {
        public SignUpUser(UserId userId, string emailAddress, string password, string firstName, string lastName,
            string street,
            string postcode, string city, string mobilePhone)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (password == null) throw new ArgumentNullException("password");
            if (firstName == null) throw new ArgumentNullException("firstName");
            if (lastName == null) throw new ArgumentNullException("lastName");

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            MobilePhone = mobilePhone;
            EmailAddress = emailAddress;
            Password = password;
        }

        public UserId UserId { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Street { get; protected set; }
        public string Postcode { get; protected set; }
        public string City { get; protected set; }
        public string MobilePhone { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string Password { get; protected set; }
    }

    public class SignUpUserHandler : IHandleCommand<SignUpUser>
    {
        private readonly IUserRepository _userRepository;

        public SignUpUserHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(SignUpUser command)
        {
            var emailAddress = command.EmailAddress.ToLowerInvariant().Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            var user = new User(command.UserId, emailAddress, command.Password, command.FirstName, command.LastName,
                command.Street,
                command.Postcode, command.City, command.MobilePhone, null);

            _userRepository.Add(user);

            EventPublisher.Publish(new UserSignedUp(user.UserId, new UserShortId(user.Id),
                user.Account.Email, user.Account.Password, user.Account.Salt, user.Account.ValidationKey, user.FirstName,
                user.LastName, user.Street, user.Postcode, user.City, user.PhoneNumber));

            ValidateAndSetRootUser(user);
        }

        private static void ValidateAndSetRootUser(User user)
        {
            if (user.EmailAddress == "admin@test.phundus.ch")
            {
                user.Account.ValidateKey(user.Account.ValidationKey);
                user.ChangeRole(new Admin(user.UserId, user.EmailAddress, user.FullName), UserRole.Admin);
            }
        }
    }
}