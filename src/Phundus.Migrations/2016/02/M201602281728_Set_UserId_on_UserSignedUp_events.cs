namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using FluentMigrator;

    [Migration(201602281728)]
    public class M201602281728_Set_UserId_on_UserSignedUp_events : EventMigrationBase
    {
        private readonly IDictionary<int, Guid> _idToGuid = new Dictionary<int, Guid>();
        private readonly IDictionary<Guid, int> _guidToId = new Dictionary<Guid, int>();

        private const string TypeName = "Phundus.IdentityAccess.Users.Model.UserSignedUp, Phundus.Core";

        protected override void Migrate()
        {
            FillMaps();
            UpdateEvents();
        }

        private void UpdateEvents()
        {
            var events = FindStoredEvents<UserSignedUp>(TypeName);

            foreach (var e in events)
            {
                if (e.UserId == Guid.Empty && e.UserShortId <= 0)
                    throw new Exception("Houston we have a problem...");

                if (e.UserId != Guid.Empty && e.UserShortId > 0)
                    continue;

                if (e.UserId == Guid.Empty)
                    e.UserId = _idToGuid[e.UserShortId];
                else if (e.UserShortId <= 0)
                    e.UserShortId = _guidToId[e.UserId];

                UpdateStoredEvent(e.EventGuid, e, e.UserId, TypeName);
            }            
        }

        private void FillMaps()
        {
            var command = CreateCommand(@"SELECT [Id], [Guid] FROM [Dm_IdentityAccess_User]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    _idToGuid.Add(reader.GetInt32(0), reader.GetGuid(1));
                    _guidToId.Add(reader.GetGuid(1), reader.GetInt32(0));
                }
            }
        }

        [DataContract]
        public class UserSignedUp : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int UserShortId { get; set; }

            [DataMember(Order = 13)]
            public Guid UserId { get; set; }

            [DataMember(Order = 2)]
            public string EmailAddress { get; private set; }

            [DataMember(Order = 3)]
            public string Password { get; private set; }

            [DataMember(Order = 4)]
            public string Salt { get; private set; }

            [DataMember(Order = 5)]
            public string ValidationKey { get; private set; }

            [DataMember(Order = 6)]
            public string FirstName { get; private set; }

            [DataMember(Order = 7)]
            public string LastName { get; private set; }

            [DataMember(Order = 8)]
            public string Street { get; private set; }

            [DataMember(Order = 9)]
            public string Postcode { get; private set; }

            [DataMember(Order = 10)]
            public string City { get; private set; }

            [DataMember(Order = 11)]
            public string PhoneNumber { get; private set; }

            [DataMember(Order = 12)]
            public int? JsNumber { get; private set; }
        }
    }
}