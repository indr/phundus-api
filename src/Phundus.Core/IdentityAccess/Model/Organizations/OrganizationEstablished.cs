namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using IdentityAccess.Model.Organizations;

    [DataContract]
    public class OrganizationEstablished : DomainEvent
    {
        public OrganizationEstablished(Founder founder, OrganizationId organizationId, string name,
            OrganizationPlan plan, bool publicRental)
        {
            if (founder == null) throw new ArgumentNullException("founder");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (name == null) throw new ArgumentNullException("name");

            Initiator = founder.ToActor();
            OrganizationId = organizationId.Id;
            Name = name;
            Plan = plan.ToString().ToLowerInvariant();
            PublicRental = publicRental;
        }

        protected OrganizationEstablished()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string Name { get; protected set; }

        [DataMember(Order = 4)]
        public string Plan { get; protected set; }

        [DataMember(Order = 5)]
        public bool PublicRental { get; protected set; }
    }
}