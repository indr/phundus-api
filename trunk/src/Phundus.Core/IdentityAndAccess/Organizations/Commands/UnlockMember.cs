namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Cqrs;
    using Repositories;
    using Users.Model;
    using Users.Repositories;

    public class UnlockMember
    {
        public int OrganizationId { get; set; }
        public int ChiefId { get; set; }
        public int MemberId { get; set; }
    }

    public class UnlockMemberHandler : IHandleCommand<UnlockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public void Handle(UnlockMember command)
        {
            var organization = OrganizationRepository.ById(command.OrganizationId);
            var chief = UserRepository.ById(command.ChiefId);
            var member = UserRepository.ById(command.MemberId);

            organization.UnlockMember(chief, member);

        }
    }
}