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
        protected IDbConnection Connection;
        protected IDbTransaction Transaction;

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

        protected IDictionary<int, Guid> GetUserIdMap()
        {
            return GetIdToGuidMap("SELECT [Id], [Guid] FROM [Dm_IdentityAccess_User]");
        } 

        protected IDictionary<int, Guid> GetOrganizationIdMap()
        {
            return new Dictionary<int, Guid>
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
        }

        protected void UpdateSerialization(long eventId, object domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);

            var command = Connection.CreateCommand();
            command.Transaction = Transaction;
            command.Parameters.Add(new SqlParameter(@"EventId", eventId));
            command.Parameters.Add(new SqlParameter(@"Serialization", stream.ToArray()));
            command.CommandText = @"UPDATE [dbo].[StoredEvents] SET [Serialization] = @Serialization WHERE [EventId] = @EventId";
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
                return Serializer.Deserialize<T>(new MemoryStream(this.Serialization));
            }
        }
    }
}