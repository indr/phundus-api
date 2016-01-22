﻿namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Cqrs;
    using Ddd;
    using Exceptions;
    using Model;
    using Repositories;

    public class SignUpUser
    {
        public SignUpUser(string emailAddress, string password, string firstName, string lastName, string street,
            string postcode, string city, string mobilePhone)
        {
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            if (password == null) throw new ArgumentNullException("password");
            if (firstName == null) throw new ArgumentNullException("firstName");
            if (lastName == null) throw new ArgumentNullException("lastName");

            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            MobilePhone = mobilePhone;
            EmailAddress = emailAddress;
            Password = password;
        }

        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Street { get; protected set; }
        public string Postcode { get; protected set; }
        public string City { get; protected set; }
        public string MobilePhone { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string Password { get; protected set; }

        [Obsolete]
        public Guid ResultingUserGuid { get; set; }
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

            var user = new User(emailAddress, command.Password, command.FirstName, command.LastName, command.Street,
                command.Postcode, command.City, command.MobilePhone, null);

            var userId = _userRepository.Add(user);

            EventPublisher.Publish(new UserSignedUp(user.UserId,
                user.Account.Email, user.Account.Password, user.Account.Salt,
                user.Account.ValidationKey,
                user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.MobileNumber
                ));

            command.ResultingUserGuid = user.Guid;
        }
    }
}