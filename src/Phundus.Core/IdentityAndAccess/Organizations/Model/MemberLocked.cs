namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MemberLocked : DomainEvent
    {
        public MemberLocked(int organizationId, int memberId)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
        }

        protected MemberLocked()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int MemberId { get; protected set; }
    }
}