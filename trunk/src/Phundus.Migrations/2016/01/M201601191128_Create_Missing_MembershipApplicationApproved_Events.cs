namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using FluentMigrator;

    [Migration(201601191128)]
    public class M201601191128_Create_Missing_MembershipApplicationApproved_Events : EventMigrationBase
    {
        protected override void Migrate()
        {
            const string typeName = @"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationApproved, Phundus.Core";
            var storedEvents = FindStoredEvents(typeName);
            var domainEvents = storedEvents.Select(s => s.Deserialize<MembershipApplicationApproved>()).ToList();

            var command = CreateCommand(@"SELECT [OrganizationGuid], [UserGuid], [ApprovalDate] FROM [Dm_IdentityAccess_Membership]");
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    var organizationGuid = reader.GetGuid(0);
                    var userGuid = reader.GetGuid(1);

                    if (domainEvents.FirstOrDefault(p => p.OrganizationGuid == organizationGuid && p.UserGuid == userGuid) != null)
                        continue;

                    var domainEvent = new MembershipApplicationApproved
                    {
                        InitiatorId = Guid.Empty,
                        OrganizationGuid = organizationGuid,
                        UserGuid = userGuid
                    };

                    InsertStoredEvent(reader.GetDateTime(2), typeName, domainEvent);
                }
        }


        [DataContract]
        internal class MembershipApplicationApproved
        {
            [DataMember(Order = 4)]
            public Guid InitiatorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrganizationGuid { get; set; }

            [DataMember(Order = 5)]
            public Guid UserGuid { get; set; }

            [Obsolete]
            [DataMember(Order = 2)]
            public int UserIntegralId { get; private set; }

            [Obsolete]
            [DataMember(Order = 1)]
            public int OrganizationIntegralId { get; private set; }
        }
    }
}