namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrganizationEstablished : DomainEvent
    {
        public OrganizationEstablished(Guid organizationid, string plan, string name, string url)
        {
            OrganizationId = organizationid;
            Plan = plan;
            Name = name;
            Url = url;
        }

        protected OrganizationEstablished()
        {
        }

        [DataMember(Order = 1)]
        public Guid OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public string Plan { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public string Url { get; set; }
    }
}