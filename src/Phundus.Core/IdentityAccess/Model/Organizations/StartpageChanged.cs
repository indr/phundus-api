namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class StartpageChanged : DomainEvent
    {
        public StartpageChanged(Initiator initiator, OrganizationId organizationId, string startpage)
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
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public string Startpage { get; set; }
    }
}