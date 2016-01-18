namespace Phundus.Migrations
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201601182024)]
    public class M201601182024_Add_UserGuid_and_OrganizationGuid_to_membership_application_events : EventMigrationBase
    {
        private StringBuilder _sb;

        protected override void Migrate()
        {
            _sb = new StringBuilder();
            Process(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationFiled, Phundus.Core");
            Process(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationRejected, Phundus.Core");
            Process(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationApproved, Phundus.Core");
            throw new Exception(_sb.ToString());
        }

        private void Process(string typeName)
        {
            
            ForEach<MembershipApplicationEvent>(typeName,
                (eventId, domainEvent) =>
                {
                    if ((domainEvent.OrganizationGuid != Guid.Empty) && (domainEvent.UserGuid != Guid.Empty))
                        return;

                    //domainEvent.OrganizationGuid = GetOrganizationGuid(domainEvent.OrganizationIntegralId);
                    //domainEvent.UserGuid = GetUserGuid(domainEvent.UserIntegralId);
                    //UpdateSerialization(eventId, domainEvent);

                    if (OrganizationIdMap.ContainsKey(domainEvent.OrganizationIntegralId))
                        return;

                    _sb.AppendLine(String.Format("Org: {0}, User {1}", domainEvent.OrganizationIntegralId,
                        domainEvent.UserIntegralId));
                });
        }

        [DataContract]
        internal class MembershipApplicationEvent : DomainEvent
        {
            [DataMember(Order = 4)]
            public Guid InitiatorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrganizationGuid { get; set; }

            [DataMember(Order = 5)]
            public Guid UserGuid { get; set; }

            [DataMember(Order = 2)]
            public int UserIntegralId { get; private set; }

            [DataMember(Order = 1)]
            public int OrganizationIntegralId { get; private set; }
        }
    }
}