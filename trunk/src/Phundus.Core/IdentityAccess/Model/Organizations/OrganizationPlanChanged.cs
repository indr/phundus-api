﻿namespace Phundus.IdentityAccess.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Users.Model;

    [DataContract]
    public class OrganizationPlanChanged : DomainEvent
    {
        public OrganizationPlanChanged(Admin admin, OrganizationId organizationId, OrganizationPlan oldPlan, OrganizationPlan newPlan)
        {
            if (admin == null) throw new ArgumentNullException("admin");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            
            Initiator = new Initiator(new InitiatorId(admin.UserId), admin.EmailAddress, admin.FullName);
            OrganizationId = organizationId.Id;
            OldPlan = oldPlan.ToString().ToLowerInvariant();
            NewPlan = newPlan.ToString().ToLowerInvariant();
        }

        protected OrganizationPlanChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string OldPlan { get; protected set; }

        [DataMember(Order = 4)]
        public string NewPlan { get; protected set; }
    }
}