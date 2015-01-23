namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using System.Globalization;
    using Common.Cqrs;
    using Cqrs;
    using Exceptions;
    using Mails;
    using Repositories;

    public class ChangeEmailAddress : ICommand
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
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangeEmailAddress command)
        {
            var user = UserRepository.GetById(command.UserId);

            var emailAddress = command.EmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (UserRepository.FindByEmail(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.Account.RequestedEmail = emailAddress;
            user.Account.GenerateValidationKey();
            UserRepository.Update(user);

            new UserChangeEmailValidationMail().For(user).Send(user);
        }
    }
}