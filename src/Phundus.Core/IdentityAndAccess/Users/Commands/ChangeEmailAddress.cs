namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using System.Globalization;
    using Cqrs;
    using Exceptions;
    using Infrastructure.Gateways;
    using Mails;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(int userId, string oldEmailAddress, string emailAddress)
        {
            UserId = userId;
            EmailAddress = emailAddress;
        }

        public int UserId { get; set; }
        public string EmailAddress { get; private set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        public IMailGateway MailGateway { get; set; }

        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangeEmailAddress command)
        {
            var user = UserRepository.GetById(command.UserId);

            var emailAddress = command.EmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (UserRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.Account.RequestedEmail = emailAddress;
            user.Account.GenerateValidationKey();
            UserRepository.Update(user);

            new UserChangeEmailValidationMail(MailGateway).For(user).Send(user);
        }
    }
}