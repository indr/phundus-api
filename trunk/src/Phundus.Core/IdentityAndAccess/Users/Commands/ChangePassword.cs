namespace Phundus.Core.IdentityAndAccess.Users.Commands
{
    using System;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;

    public class ChangePassword
    {
        public ChangePassword(UserId initiatorId, string oldPassword, string newPassword)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");

            InitiatorId = initiatorId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public UserId InitiatorId { get; private set; }
        public string OldPassword { get; private set; }
        public string NewPassword { get; private set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        [Transaction]
        public void Handle(ChangePassword command)
        {
            var user = UserRepository.GetById(command.InitiatorId);

            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}