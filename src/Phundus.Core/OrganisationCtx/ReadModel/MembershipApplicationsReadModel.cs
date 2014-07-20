namespace Phundus.Core.OrganisationCtx.ReadModel
{
    using System;
    using System.Collections.Generic;
    using Ddd;
    using DomainModel;

    public class MembershipApplicationsReadModel : ISubscribeTo<MembershipRequested>,
        ISubscribeTo<MembershipRequestApproved>, ISubscribeTo<MembershipRequestRejected>
    {
        public void Handle(MembershipRequestApproved @event)
        {
            throw new NotImplementedException();
        }

        public void Handle(MembershipRequested @event)
        {
            throw new NotImplementedException();
        }

        public void Handle(MembershipRequestRejected @event)
        {
            throw new NotImplementedException();
        }

        public MembershipApplicationDtos ByOrganization(int orgId)
        {
            return new MembershipApplicationDtos();
        }
    }

    public class MembershipApplicationDto
    {
        public int OrgId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class MembershipApplicationDtos : List<MembershipApplicationDto>
    {
        
    }
}