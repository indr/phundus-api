namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Users;

    [DataContract]
    public class StartpageChanged : DomainEvent
    {
        public StartpageChanged(Manager initiator, OrganizationId organizationId, string startpage)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");

            Initiator = initiator;
            OrganizationId = organizationId.Id;
            Startpage = startpage;
        }

        protected StartpageChanged()
        {
        }

        [DataMember(Order = 1)]
        public Manager Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string Startpage { get; protected set; }
    }
}