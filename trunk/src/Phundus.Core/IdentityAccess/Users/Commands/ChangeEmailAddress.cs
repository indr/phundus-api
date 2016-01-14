namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using IdentityAccess.Users.Repositories;
    using Infrastructure.Gateways;
    using Mails;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(UserId initiatorId, string password, string newEmailAddress)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            InitiatorId = initiatorId;
            Password = password;
            NewEmailAddress = newEmailAddress;
        }

        public UserId InitiatorId { get; protected set; }
        public string Password { get; protected set; }
        public string NewEmailAddress { get; protected set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        public IMailGateway MailGateway { get; set; }

        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangeEmailAddress command)
        {
            var user = UserRepository.GetById(command.InitiatorId);

            var emailAddress = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (UserRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.Account.ChangeEmailAddress(command.Password, command.NewEmailAddress);
            user.Account.GenerateValidationKey();
            UserRepository.Update(user);

            new UserChangeEmailValidationMail(MailGateway).For(user).Send(user);
        }
    }
}