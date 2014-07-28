namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Castle.Transactions;
    using NHibernate.Transform;
    using Organizations.Model;
    using Users.Model;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationDto> PendingByOrganizationId(int organizationId);
    }

    public class MembershipApplicationsReadModel : ReadModelBase, IMembershipApplicationQueries
    {
        [Transaction]
        public IList<MembershipApplicationDto> PendingByOrganizationId(int organizationId)
        {
            MembershipRequest membershipRequest = null;
            User user = null;
            Account account = null;
            MembershipApplicationDto dto = null;

            var result = Session.QueryOver(() => membershipRequest)
                .Where(p => p.OrganizationId == organizationId)
                .And(p => p.ApprovalDate == null)
                .And(p => p.RejectDate == null)
                .JoinAlias(() => membershipRequest.User, () => user)
                .JoinAlias(() => user.Account, () => account)
                .SelectList(list => list
                    .Select(r => r.Id).WithAlias(() => dto.Id)
                    .Select(r => r.OrganizationId).WithAlias(() => dto.OrganizationId)
                    .Select(r => r.UserId).WithAlias(() => dto.UserId)
                    .Select(r => user.FirstName).WithAlias(() => dto.FirstName)
                    .Select(r => user.LastName).WithAlias(() => dto.LastName)
                    .Select(r => account.Email).WithAlias(() => dto.Email)
                    .Select(r => user.JsNumber).WithAlias(() => dto.JsNumber)
                    .Select(r => r.RequestDate).WithAlias(() => dto.CreatedOn)
                    .Select(r => r.ApprovalDate).WithAlias(() => dto.ApprovedOn)
                    .Select(r => r.RejectDate).WithAlias(() => dto.RejectedOn)
                )
                .TransformUsing(Transformers.AliasToBean<MembershipApplicationDto>()).List<MembershipApplicationDto>();

            return result;
        }

        private static MembershipApplicationDto ToMembershipApplicationDto(MembershipRequest membershipRequest,
            UserDto user)
        {
            return new MembershipApplicationDto
            {
                Id = membershipRequest.Id,
                OrganizationId = membershipRequest.OrganizationId,
                UserId = membershipRequest.UserId,
                CreatedOn = membershipRequest.RequestDate,
                ApprovedOn = membershipRequest.ApprovalDate,
                RejectedOn = membershipRequest.RejectDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                JsNumber = user.JsNumber
            };
        }
    }

    public class MembershipApplicationDto
    {
        public Guid Id { get; set; }

        public int OrganizationId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? JsNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public DateTime? RejectedOn { get; set; }
    }
}