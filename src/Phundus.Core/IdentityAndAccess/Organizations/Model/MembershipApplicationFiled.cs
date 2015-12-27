namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationFiled : DomainEvent
    {
        protected MembershipApplicationFiled()
        {
        }

        public MembershipApplicationFiled(int organizationId, int userId, Guid organizationGuid)
        {
            OrganizationId = organizationId;
            UserId = userId;
            OrganizationGuid = organizationGuid;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int UserId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid OrganizationGuid { get; protected set; }
    }
}