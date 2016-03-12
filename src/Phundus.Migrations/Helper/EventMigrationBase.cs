namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Common.Domain.Model;
    using ProtoBuf;

    public class MigratingDomainEvent
    {
        public Guid EventGuid { get; set; }

        public DateTime OccuredOnUtc { get; set; }
    }

    public abstract class EventMigrationBase : MigrationBase
    {
        protected static IDictionary<int, Guid> OrganizationIdMap = new Dictionary<int, Guid>
        {
            {1000, new Guid("1E2311AD-2340-4AB1-BE0E-54DA9658FBD7")},
            {1001, new Guid("9E327414-8BDC-42E5-A711-3A15694C0026")},
            {1002, new Guid("4E5F3B71-9FCC-4CC6-AC66-B0258861C3E9")},
            {1003, new Guid("393FCA14-71F8-4348-A0E5-7F6E6C025339")},
            //{1004, new Guid("8B657CC6-61DE-4F84-A5E2-3EC08E9FF487")}, // Nicht sicher
            {1005, new Guid("08308E87-58D6-43CD-A583-72F8A83AC15D")},
            {1006, new Guid("428D069D-1183-4643-BECF-276A9BC523BE")},
            //{1007, new Guid("3B148DC3-AE2C-4486-9D63-BEB5A9BE320E")} // Stimmt nicht
            {1009, new Guid("3B148DC3-AE2C-4486-9D63-BEB5A9BE320E")} // Ludothek Luzern
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

        private readonly IList<IDbCommand> _commands = new List<IDbCommand>();

        public override void Up()
        {
            Execute.WithConnection((connection, transaction) =>
            {
                Connection = connection;
                Transaction = transaction;
                Migrate();
                foreach (var eachCommand in _commands)
                    eachCommand.ExecuteNonQuery();
            });
        }

        protected IDbCommand CreateCommand(string commandText)
        {
            var command = Connection.CreateCommand();
            command.Transaction = Transaction;
            command.CommandText = commandText;
            return command;
        }

        protected abstract void Migrate();

        protected void Reinsert(string typeName)
        {
            var events = FindStoredEvents(typeName);
            foreach (var se in events)
            {
                DeleteStoredEvent(se.EventGuid);
                InsertStoredEvent(se.OccuredOnUtc, typeName, se.Serialization, se.AggregateId, se.EventGuid);
            }
        }

        protected void DeleteStoredEvent(Guid eventGuid)
        {
            var command = CreateCommand(@"DELETE FROM [StoredEvents] WHERE [EventGuid] = @EventGuid");
            command.Parameters.Add(new SqlParameter("@EventGuid", eventGuid));
            _commands.Add(command);
        }

        protected void DeleteStoredEvents(string typeName)
        {
            var command = CreateCommand(@"DELETE FROM [StoredEvents] WHERE [TypeName] = @TypeName");
            command.Parameters.Add(new SqlParameter(@"TypeName", typeName));
            _commands.Add(command);
        }

        protected void InsertStoredEvent(DateTime occuredOnUtc, string typeName, object domainEvent, Guid? aggregateId = null, Guid? eventGuid = null)
        {
            InsertStoredEvent(occuredOnUtc, typeName, Serialize(domainEvent), aggregateId);
        }

        private void InsertStoredEvent(DateTime occuredOnUtc, string typeName, byte[] serialization, Guid? aggregateId = null, Guid? eventGuid = null)
        {
            var command = CreateCommand(@"
INSERT INTO [StoredEvents] ([EventGuid], [TypeName], [OccuredOnUtc], [AggregateId], [Serialization])
VALUES (@EventGuid, @TypeName, @OccuredOnUtc, @AggregateId, @Serialization)");
            command.Parameters.Add(new SqlParameter("@EventGuid", eventGuid == null ? Guid.NewGuid() : eventGuid.Value));
            command.Parameters.Add(new SqlParameter("@TypeName", typeName));
            command.Parameters.Add(new SqlParameter("@OccuredOnUtc", occuredOnUtc));
            command.Parameters.Add(new SqlParameter("@AggregateId", aggregateId.HasValue ? aggregateId.Value : Guid.Empty));
            command.Parameters.Add(new SqlParameter("@Serialization", serialization));

            _commands.Add(command);
        }

        protected void UpdateSerialization(long eventId, object domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);

            var command = CreateCommand(
                    @"UPDATE [dbo].[StoredEvents] SET [Serialization] = @Serialization WHERE [EventId] = @EventId");
            command.Parameters.Add(new SqlParameter(@"EventId", eventId));
            command.Parameters.Add(new SqlParameter(@"Serialization", stream.ToArray()));
            _commands.Add(command);
        }

        protected void UpdateStoredEvent(Guid eventGuid, MigratingDomainEvent domainEvent, Guid? aggregateId = null, string typeName = null)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);

            var setTypeNameSql = "";
            SqlParameter setTypeNameParameter = null;
            if (!String.IsNullOrWhiteSpace(typeName))
            {
                setTypeNameSql = ", [TypeName] = @TypeName";
                setTypeNameParameter = new SqlParameter(@"TypeName", typeName);
            }

            var command = CreateCommand(@"
UPDATE [dbo].[StoredEvents] SET
  [Serialization] = @Serialization
, [AggregateId] = @AggregateId
" + setTypeNameSql + @"
WHERE [EventGuid] = @EventGuid");
            command.Parameters.Add(new SqlParameter(@"EventGuid", eventGuid));
            command.Parameters.Add(new SqlParameter(@"Serialization", stream.ToArray()));
            command.Parameters.Add(new SqlParameter(@"AggregateId",
                aggregateId.HasValue ? aggregateId.Value : Guid.Empty));
            if (setTypeNameParameter != null)
                command.Parameters.Add(setTypeNameParameter);
            _commands.Add(command);
        }

        protected void UpdateStoredEventAggregateId(Guid eventGuid, Guid aggregateId)
        {
            var command = CreateCommand(
                    @"UPDATE [dbo].[StoredEvents] SET [AggregateId] = @AggregateId WHERE [EventGuid] = @EventGuid");
            command.Parameters.Add(new SqlParameter(@"EventGuid", eventGuid));
            command.Parameters.Add(new SqlParameter(@"AggregateId", aggregateId));
            _commands.Add(command);
        }

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

        internal IList<TDomainEvent> FindStoredEvents<TDomainEvent>(string typeName) where TDomainEvent : MigratingDomainEvent
        {
            return FindStoredEvents(typeName).Select(s => Deserialize<TDomainEvent>(s)).ToList();
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

        internal void ForEach(string typeName, Action<StoredEvent> action)
        {
            var storedEvents = FindStoredEvents(typeName);
            foreach (var storedEvent in storedEvents)
            {
                action(storedEvent);
            }
        }

        internal void ForEach<TDomainEvent>(string typeName, Action<long, TDomainEvent> action) where TDomainEvent : MigratingDomainEvent
        {
            var storedEvents = FindStoredEvents(typeName);
            foreach (var storedEvent in storedEvents)
            {
                TDomainEvent domainEvent;
                try
                {
                    domainEvent = Deserialize<TDomainEvent>(storedEvent);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format(@"Error deserializing event {0}, event type name ""{1}"", requested domain event type ""{2}"".",
                     storedEvent.EventGuid, storedEvent.TypeName, typeof(TDomainEvent).FullName + ", " + typeof(TDomainEvent).Assembly.GetName().Name), ex);
                }
                action(storedEvent.EventId, domainEvent);
            }
        }

        internal T Deserialize<T>(StoredEvent storedEvent) where T : MigratingDomainEvent
        {
            var result = Serializer.Deserialize<T>(new MemoryStream(storedEvent.Serialization));
            result.EventGuid = storedEvent.EventGuid;
            result.OccuredOnUtc = storedEvent.OccuredOnUtc;
            return result;
        }

        internal byte[] Serialize(object domainEvent)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, domainEvent);
            return stream.ToArray();
        }

        internal class StoredEvent
        {
            public Guid AggregateId;
            public Guid EventGuid;
            public long EventId;
            public DateTime OccuredOnUtc;
            public byte[] Serialization;
            public string TypeName;

            public T Deserialize<T>() where T : DomainEvent
            {
                var domainEvent = Serializer.Deserialize<T>(new MemoryStream(Serialization));
                if (domainEvent == null)
                    throw new InvalidOperationException("Error deserializing event.");
                EventGuidProperty.SetValue(domainEvent, EventGuid, null);
                OccuredOnUtcProp.SetValue(domainEvent, OccuredOnUtc, null);
                return domainEvent;
            }

            private static readonly PropertyInfo EventGuidProperty = typeof(DomainEvent).GetProperty("EventGuid");
            private static readonly PropertyInfo OccuredOnUtcProp = typeof(DomainEvent).GetProperty("OccuredOnUtc");
        }
    }
}