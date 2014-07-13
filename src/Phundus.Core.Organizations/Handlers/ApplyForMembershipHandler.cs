namespace Phundus.Core.Organizations.Handlers
{
    using Commands;
    using Infrastructure.Cqrs;

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        //public IOrganizationRepository Organizations { get; set; }

        //public IMemberRepository Members { get; set; }

        public void Handle(ApplyForMembership command)
        {
            //Organization organization = Organizations.FindById(command.OrganizationId);

            //User user = Members.FindById(command.MemberId);

            //user.Join(organization);
        }
    }
}