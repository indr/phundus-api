namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using Castle.Transactions;
    using Cqrs;
    using Repositories;

    public class ChangePassword
    {
        public ChangePassword(string username, string oldPassword, string newPassword)
        {
            Username = username;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string NewPassword { get; private set; }

        public string OldPassword { get; private set; }

        public string Username { get; private set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        [Transaction]
        public void Handle(ChangePassword command)
        {
            var user = UserRepository.FindByEmailAddress(command.Username);
            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}