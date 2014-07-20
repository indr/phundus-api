namespace Phundus.Core.OrganisationCtx.ReadModel
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using Ddd;
    using DomainModel;
    using Repositories;

    public interface IMembershipApplicationsReadModel
    {
        MembershipApplicationDtos ByOrganization(int organizationId);
    }

    public class MembershipApplicationsReadModel : IMembershipApplicationsReadModel,
            ISubscribeTo<MembershipRequested>,
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

        public IMembershipRequestRepository MembershipRequestRepository { get; set; }

        [Transaction]
        public MembershipApplicationDtos ByOrganization(int organizationId)
        {
            var requests = MembershipRequestRepository.ByOrganization(organizationId);

            var result = new MembershipApplicationDtos();

            foreach (var each in requests)
            {
                result.Add(new MembershipApplicationDto
                {
                    OrgId = each.OrganizationId,
                    UserId = each.MemberId,
                    CreatedOn = each.RequestDate
                });
            }

            return result;
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