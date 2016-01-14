namespace Phundus.IdentityAccess.Organizations.Model
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

        public MembershipApplicationFiled(Guid organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        [DataMember(Order = 3)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int UserId { get; protected set; }
    }
}