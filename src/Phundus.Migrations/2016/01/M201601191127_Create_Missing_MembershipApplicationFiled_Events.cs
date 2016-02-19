namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201601191127)]
    public class M201601191127_Create_Missing_MembershipApplicationFiled_Events : EventMigrationBase
    {
        protected override void Migrate()
        {
            const string typeName = @"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationFiled, Phundus.Core";
            var storedEvents = FindStoredEvents(typeName);
            var domainEvents = storedEvents.Select(s => s.Deserialize<MembershipApplicationFiled>()).ToList();
            
            var command = CreateCommand(@"SELECT [OrganizationGuid], [UserGuid], [ApprovalDate] FROM [Dm_IdentityAccess_Membership]");
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    var organizationGuid = reader.GetGuid(0);
                    var userGuid = reader.GetGuid(1);

                    if (domainEvents.FirstOrDefault(p => p.OrganizationGuid == organizationGuid && p.UserGuid == userGuid) != null)
                        continue;

                    var domainEvent = new MembershipApplicationFiled
                    {
                        InitiatorId = Guid.Empty,
                        OrganizationGuid = organizationGuid,
                        UserGuid = userGuid
                    };
                    
                    InsertStoredEvent(reader.GetDateTime(2), typeName, domainEvent);
                }
        }

        
        [DataContract]
        internal class MembershipApplicationFiled : DomainEvent
        {
            [DataMember(Order = 4)]
            public Guid InitiatorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrganizationGuid { get; set; }

            [DataMember(Order = 5)]
            public Guid UserGuid { get; set; }

            [Obsolete]
            [DataMember(Order = 1)]
            public int OrganizationShortId { get; private set; }

            [Obsolete]
            [DataMember(Order = 2)]
            public int UserShortId { get; private set; }
        }
    }
}