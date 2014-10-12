﻿namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System.Runtime.Serialization;
    using Ddd;

    [DataContract]
    public class MembershipApplicationFiled : DomainEvent
    {
        public MembershipApplicationFiled(int organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int UserId { get; protected set; }
    }
}