namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Phundus.IdentityAccess.Model.Users;

    [DataContract]
    public class OrganizationRenamed : DomainEvent
    {
        public OrganizationRenamed(Manager initiator, OrganizationId organizationId, string name)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (name == null) throw new ArgumentNullException("name");

            Initiator = initiator.ToActor();
            OrganizationId = organizationId.Id;
            Name = name;
        }

        protected OrganizationRenamed()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string Name { get; protected set; }
    }
}
