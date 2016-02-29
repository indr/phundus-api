namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602290949)]
    public class M201602290949_Generate_missing_OrganizationContactDetailsChanged_events : EventMigrationBase
    {
        private const string TypeName = @"Phundus.IdentityAccess.Model.Organizations.OrganizationContactDetailsChanged, Phundus.Core";

        protected override void Migrate()
        {
            var events = FindStoredEvents<OrganizationContactDetailsChanged>(TypeName);
            var command = CreateCommand(@"
SELECT [Guid] 
      ,[Line1]
      ,[Line2]
      ,[Street]
      ,[Postcode]
      ,[City]
      ,[EmailAddress]
      ,[PhoneNumber]      
      ,[Website]
      ,[CreateDate]
  FROM [Dm_IdentityAccess_Organization]");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var organizationId = reader.GetGuid(0);
                    if (events.Count(p => p.OrganizationId == organizationId) > 0)
                        continue;

                    var e = new OrganizationContactDetailsChanged();
                    e.OrganizationId = organizationId;
                    e.Line1 = reader.GetStringOrNull(1);
                    e.Line2 = reader.GetStringOrNull(2);
                    e.Street = reader.GetStringOrNull(3);
                    e.Postcode = reader.GetStringOrNull(4);
                    e.City = reader.GetStringOrNull(5);
                    e.EmailAddress = reader.GetStringOrNull(6);
                    e.PhoneNumber = reader.GetStringOrNull(7);
                    e.Website = reader.GetStringOrNull(8);
                    var createdAtUtc = reader.GetDateTime(9).ToUniversalTime();

                    InsertStoredEvent(createdAtUtc, TypeName, e, e.OrganizationId);
                }
            }
        }

        [DataContract]
        public class OrganizationContactDetailsChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 3)]
            public string Line1 { get; set; }

            [DataMember(Order = 4)]
            public string Line2 { get; set; }

            [DataMember(Order = 5)]
            public string Street { get; set; }

            [DataMember(Order = 6)]
            public string Postcode { get; set; }

            [DataMember(Order = 7)]
            public string City { get; set; }

            [DataMember(Order = 8)]
            public string PhoneNumber { get; set; }

            [DataMember(Order = 9)]
            public string EmailAddress { get; set; }

            [DataMember(Order = 10)]
            public string Website { get; set; }
        }

        [DataContract]
        public class Initiator
        {
            public Initiator(InitiatorId initiatorId, string emailAddress, string fullName)
            {
                if (initiatorId == null) throw new ArgumentNullException("initiatorId");
                if (fullName == null) throw new ArgumentNullException("fullName");
                if (emailAddress == null) throw new ArgumentNullException("emailAddress");

                InitiatorId = initiatorId;
                EmailAddress = emailAddress;
                FullName = fullName;
            }

            protected Initiator()
            {
            }

            public InitiatorId InitiatorId { get; protected set; }

            [DataMember(Order = 1)]
            protected Guid InitiatorGuid
            {
                get { return InitiatorId.Id; }
                set { InitiatorId = new InitiatorId(value); }
            }

            [DataMember(Order = 2)]
            public string EmailAddress { get; protected set; }

            [DataMember(Order = 3)]
            public string FullName { get; protected set; }
        }
    }
}