namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Cqrs;
    using Repositories;
    using Users.Repositories;

    public class LockMember
    {
        public int OrganizationId { get; set; }
        public int ChiefId { get; set; }
        public int MemberId { get; set; }
    }

    public class LockMemberHandler : IHandleCommand<LockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public void Handle(LockMember command)
        {
            var organization = OrganizationRepository.ById(command.OrganizationId);
            var chief = UserRepository.ById(command.ChiefId);
            var member = UserRepository.ById(command.MemberId);

            organization.LockMember(chief, member);
        }
    }
}