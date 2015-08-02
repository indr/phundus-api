namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using Cqrs;
    using Ddd;
    using Exceptions;
    using Model;
    using Repositories;

    public class RegisterUser
    {
        public RegisterUser(string emailAddress, string password, string firstName, string lastName, string street,
            string postcode, string city, string mobilePhone)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            MobilePhone = mobilePhone;
            EmailAddress = emailAddress;
            Password = password;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Street { get; private set; }
        public string Postcode { get; private set; }
        public string City { get; private set; }
        public string MobilePhone { get; private set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }

        public int UserId { get; set; }
    }

    public class RegisterUserHandler : IHandleCommand<RegisterUser>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(RegisterUser command)
        {
            var emailAddress = command.EmailAddress.ToLowerInvariant().Trim();
            if (UserRepository.FindByEmail(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            var user = new User(emailAddress, command.Password, command.FirstName, command.LastName, command.Street,
                command.Postcode, command.City, command.MobilePhone, null);

            var userId = UserRepository.Add(user);

            EventPublisher.Publish(new UserRegistered(userId,
                user.Account.Email, user.Account.Password, user.Account.Salt,
                user.Account.ValidationKey, 
                user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.MobileNumber
                ));

            command.UserId = userId;
        }
    }
}