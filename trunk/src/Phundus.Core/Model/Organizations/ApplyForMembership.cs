namespace Phundus.Core.Model.Organizations
{
    using Cqrs;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;


    public class ApplyForMembership
    {
        public int MemberId { get; set; }
        public int OrganizationId { get; set; }
    }

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