namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using ProtoBuf;

    public abstract class EventMigrationBase : MigrationBase
    {
        private static IDictionary<int, Guid> OrganizationIdMap = new Dictionary<int, Guid>
        {
            {1000, new Guid("1E2311AD-2340-4AB1-BE0E-54DA9658FBD7")},
            {1001, new Guid("9E327414-8BDC-42E5-A711-3A15694C0026")},
            {1002, new Guid("4E5F3B71-9FCC-4CC6-AC66-B0258861C3E9")},
            {1003, new Guid("393FCA14-71F8-4348-A0E5-7F6E6C025339")},
            {1004, new Guid("8B657CC6-61DE-4F84-A5E2-3EC08E9FF487")},
            {1005, new Guid("08308E87-58D6-43CD-A583-72F8A83AC15D")},
            {1006, new Guid("428D069D-1183-4643-BECF-276A9BC523BE")},
            {1007, new Guid("3B148DC3-AE2C-4486-9D63-BEB5A9BE320E")}
        };

        protected IDbConnection Connection;
        protected IDbTransaction Transaction;
        private IDictionary<int, Guid> _userIdMap;

        private IDictionary<int, Guid> UserIdMap
        {
            get
            {
                if (_userIdMap == null)
                    _userIdMap = GetIdToGuidMap("SELECT [Id], [Guid] FROM [Dm_IdentityAccess_User]");
                return _userIdMap;
            }
        }

        public override void Up()
        {
            Execute.WithConnection((connection, transaction) =>
            {
                Connection = connection;
                Transaction = transaction;
                Migrate();
            });
        }

        protected abstract void Migrate();

        protected Guid GetOrganizationGuid(int organizationIntegralId)
        {
            Guid result;
            if (!OrganizationIdMap.TryGetValue(organizationIntegralId, out result))
                throw new Exception(String.Format("Could not find the organization guid for integral id {0}.",
                    organizationIntegralId));
            return result;
        }

        protected Guid GetUserGuid(int userIntegralId)
        {
            Guid result;
            if (!UserIdMap.TryGetValue(userIntegralId, out result))
                throw new Exception(String.Format("Could not find the user guid for integral id {0}.", userIntegralId));
            return result;
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        private IDictionary<int, Guid> GetIdToGuidMap(string selectSql)
        {
            var result = new Dictionary<int, Guid>();
            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            cmd.CommandText = selectSql;

            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    result.Add(reader.GetInt32(0), reader.GetGuid(1));
            return result;
        }


        protected void UpdateSerialization(long eventId, object domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);

            var command = Connection.CreateCommand();
            command.Transaction = Transaction;
            command.Parameters.Add(new SqlParameter(@"EventId", eventId));
            command.Parameters.Add(new SqlParameter(@"Serialization", stream.ToArray()));
            command.CommandText =
                @"UPDATE [dbo].[StoredEvents] SET [Serialization] = @Serialization WHERE [EventId] = @EventId";
            command.ExecuteNonQuery();
        }

        internal IList<StoredEvent> FindStoredEvents(string typeName)
        {
            var result = new List<StoredEvent>();

            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            cmd.Parameters.Add(new SqlParameter("@TypeName", typeName));
            cmd.CommandText = @"
SELECT [EventId], [EventGuid], [TypeName], [OccuredOnUtc], [AggregateId], [Serialization]
FROM [dbo].[StoredEvents]
WHERE [TypeName] = @TypeName";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new StoredEvent
                    {
                        EventId = reader.GetInt64(0),
                        EventGuid = reader.GetGuid(1),
                        TypeName = reader.GetString(2),
                        OccuredOnUtc = reader.GetDateTime(3),
                        AggregateId = reader.GetGuid(4),
                        Serialization = (byte[]) reader.GetValue(5)
                    });
                }
            }

            return result;
        }

        internal void ForEach<TDomainEvent>(string typeName, Action<long, TDomainEvent> action)
        {
            var storedEvents = FindStoredEvents(typeName);
            foreach (var storedEvent in storedEvents)
            {
                action(storedEvent.EventId, Deserialize<TDomainEvent>(storedEvent.Serialization));
            }
        }

        internal T Deserialize<T>(byte[] serialization)
        {
            return Serializer.Deserialize<T>(new MemoryStream(serialization));
        }

        internal class StoredEvent
        {
            public Guid AggregateId;
            public Guid EventGuid;
            public long EventId;
            public DateTime OccuredOnUtc;
            public byte[] Serialization;
            public string TypeName;

            public T Deserialize<T>()
            {
                return Serializer.Deserialize<T>(new MemoryStream(Serialization));
            }
        }
    }
}