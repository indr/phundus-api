namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class ChangePassword
    {
        public ChangePassword(UserGuid initiatorGuid, string oldPassword, string newPassword)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");

            InitiatorGuid = initiatorGuid;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public UserGuid InitiatorGuid { get; private set; }
        public string OldPassword { get; private set; }
        public string NewPassword { get; private set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangePassword command)
        {
            var user = UserRepository.GetByGuid(command.InitiatorGuid);

            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}