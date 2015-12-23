namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrganizationEstablished : DomainEvent
    {
        public OrganizationEstablished(int organizationid, string name, string url, int founderId)
        {
            OrganizationId = organizationid;
            Name = name;
            Url = url;
            FounderId = founderId;
        }

        protected OrganizationEstablished()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string Url { get; set; }

        [DataMember(Order = 4)]
        public int FounderId { get; set; }
    }
}