namespace Phundus.Migrations
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using FluentMigrator;

    [Migration(201602281804)]
    public class M201602281804_Generate_missing_UserSignedUp_events : EventMigrationBase
    {
        private const string TypeName = "Phundus.IdentityAccess.Users.Model.UserSignedUp, Phundus.Core";

        protected override void Migrate()
        {
            var events = FindStoredEvents<UserSignedUp>(TypeName);
            var command = CreateCommand(@"
SELECT u.[Id]
      ,[Guid]
      ,[FirstName]
      ,[LastName]
      ,[Street]
      ,[Postcode]
      ,[City]
      ,[MobileNumber]
      ,a.Email
      ,[JsNumber]
      ,a.Salt
      ,a.Password     
      ,a.ValidationKey
      ,a.CreateDate
  FROM [Dm_IdentityAccess_User] u
  LEFT OUTER JOIN [Dm_IdentityAccess_Account] a ON a.Id = u.Id");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var userShortId = reader.GetInt32(0);
                    var userId = reader.GetGuid(1);

                    if (events.SingleOrDefault(p => p.UserId == userId) != null)
                        return;

                    var e = new UserSignedUp();
                    e.UserId = userId;
                    e.UserShortId = userShortId;
                    e.FirstName = reader.IsDBNull(2) ? null : reader.GetString(2);
                    e.LastName = reader.IsDBNull(3) ? null : reader.GetString(3);
                    e.Street = reader.IsDBNull(4) ? null : reader.GetString(4);
                    e.Postcode = reader.IsDBNull(5) ? null : reader.GetString(5);
                    e.City = reader.IsDBNull(6) ? null : reader.GetString(6);
                    e.PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7);
                    e.EmailAddress = reader.IsDBNull(8) ? null : reader.GetString(8);
                    e.JsNumber = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9);
                    e.Salt = reader.IsDBNull(10) ? null : reader.GetString(10);
                    e.Password = reader.IsDBNull(11) ? null : reader.GetString(11);
                    e.ValidationKey = reader.IsDBNull(12) ? null : reader.GetString(12);
                    var createdAtUtc = reader.GetDateTime(13).ToUniversalTime();

                    InsertStoredEvent(createdAtUtc, TypeName, e, e.UserId);
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
            public string EmailAddress { get; set; }

            [DataMember(Order = 3)]
            public string Password { get; set; }

            [DataMember(Order = 4)]
            public string Salt { get; set; }

            [DataMember(Order = 5)]
            public string ValidationKey { get; set; }

            [DataMember(Order = 6)]
            public string FirstName { get; set; }

            [DataMember(Order = 7)]
            public string LastName { get; set; }

            [DataMember(Order = 8)]
            public string Street { get; set; }

            [DataMember(Order = 9)]
            public string Postcode { get; set; }

            [DataMember(Order = 10)]
            public string City { get; set; }

            [DataMember(Order = 11)]
            public string PhoneNumber { get; set; }

            [DataMember(Order = 12)]
            public int? JsNumber { get; set; }
        }
    }
}