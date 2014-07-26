namespace Phundus.Core.IdentityAndAccessCtx.Commands
{
    using System.Globalization;
    using Cqrs;
    using Exceptions;
    using Mails;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(string oldEmailAddress, string newEmailAddress)
        {
            OldEmailAddress = oldEmailAddress;
            NewEmailAddress = newEmailAddress;
        }

        public string NewEmailAddress { get; private set; }

        public string OldEmailAddress { get; private set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangeEmailAddress command)
        {
            var email = command.OldEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            var newEmail = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();

            if (UserRepository.FindByEmail(newEmail) != null)
                throw new EmailAlreadyTakenException();

            var user = UserRepository.FindByEmail(email);
            user.Account.RequestedEmail = newEmail;
            user.Account.GenerateValidationKey();
            UserRepository.Update(user);

            new UserChangeEmailValidationMail().For(user).Send(user);
        }
    }
}