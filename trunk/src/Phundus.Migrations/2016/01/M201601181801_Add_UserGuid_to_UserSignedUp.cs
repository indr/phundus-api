﻿namespace Phundus.Migrations
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;
    using ProtoBuf;

    [Migration(201601181801)]
    public class M201601181801_Add_UserGuid_to_UserSignedUp : EventMigrationBase
    {
        protected override void Migrate()
        {
            var userIdMap = GetUserIdMap();
            var storedEvents = FindStoredEvents(@"Phundus.IdentityAccess.Users.Model.UserLoggedIn, Phundus.Core");
            foreach (var storedEvent in storedEvents)
            {

                var domainEvent = storedEvent.Deserialize<UserSignedUp>();
                if (domainEvent.UserGuid == Guid.Empty)
                {
                    domainEvent.UserGuid = userIdMap[domainEvent.UserIntegralId];
                    UpdateSerialization(storedEvent.EventId, domainEvent);
                }
            }
        }

        [DataContract]
        internal class UserSignedUp : DomainEvent
        {
            [DataMember(Order = 1)]
            public int UserIntegralId { get; set; }

            [DataMember(Order = 13)]
            public Guid UserGuid { get; set; }

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
            public string MobilePhone { get; private set; }

            [DataMember(Order = 12)]
            public int? JsNumber { get; private set; }
        }
    }
}