namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Users;

    public class ChangePassword
    {
        public ChangePassword(InitiatorId initiatorId, string oldPassword, string newPassword)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (oldPassword == null) throw new ArgumentNullException("oldPassword");
            if (newPassword == null) throw new ArgumentNullException("newPassword");

            InitiatorId = initiatorId;
            UserId = new UserId(initiatorId.Id);
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
        public string OldPassword { get; protected set; }
        public string NewPassword { get; protected set; }
    }

    public class ChangePasswordHandler : IHandleCommand<ChangePassword>
    {
        public IUserRepository UserRepository { get; set; }

        public void Handle(ChangePassword command)
        {
            var user = UserRepository.GetById(command.UserId);

            user.Account.ChangePassword(command.OldPassword, command.NewPassword);
        }
    }
}