namespace Phundus.Migrations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602290942)]
    public class M201602290942_Generate_StartpageChanged_events : EventMigrationBase
    {
        private const string TypeName = @"Phundus.IdentityAccess.Model.Organizations.StartpageChanged, Phundus.Core";

        protected override void Migrate()
        {
            var command =
                CreateCommand(@"SELECT [Guid], [Startpage], [CreateDate] FROM [Dm_IdentityAccess_Organization]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var organizationId = reader.GetGuid(0);
                    var startpage = reader.IsDBNull(1) ? null : reader.GetString(1);
                    if (startpage == null)
                        continue;
                    var createdAtUc = reader.GetDateTime(2).ToUniversalTime();

                    var e = new StartpageChanged();
                    e.OrganizationId = organizationId;
                    e.Startpage = startpage;

                    InsertStoredEvent(createdAtUc, TypeName, e, e.OrganizationId);
                }
            }
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

        [DataContract]
        public class StartpageChanged : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Initiator Initiator { get; set; }

            [DataMember(Order = 2)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 3)]
            public string Startpage { get; set; }
        }
    }
}