namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using Infrastructure.Gateways;
    using Mails;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(UserId initiatorId, string newEmailAddress)
        {
            InitiatorId = initiatorId;
            NewEmailAddress = newEmailAddress;
        }

        public UserId InitiatorId { get; set; }
        public string NewEmailAddress { get; private set; }
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

            user.Account.RequestedEmail = emailAddress;
            user.Account.GenerateValidationKey();
            UserRepository.Update(user);

            new UserChangeEmailValidationMail(MailGateway).For(user).Send(user);
        }
    }
}