namespace Phundus.Migrations
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201601190635)]
    public class M201601190635_Migrate_member_locked_and_unlocked : EventMigrationBase
    {
        private StringBuilder _sb;

        private DateTime Rev_1387 = new DateTime(2015, 12, 26, 09, 34, 00);
        private DateTime Rev_1506 = new DateTime(2016, 01, 07, 09, 03, 00);

        protected override void Migrate()
        {
            _sb = new StringBuilder();

            ProcessLocked(@"Phundus.IdentityAccess.Organizations.Model.MemberLocked, Phundus.Core");
            ProcessUnlocked(@"Phundus.IdentityAccess.Organizations.Model.MemberUnlocked, Phundus.Core");
            

            if (_sb.Length > 0)
                throw new Exception(_sb.ToString());
        }

        private void ProcessLocked(string typeName)
        {
            ForEach(typeName, storedEvent =>
            {
                var current = new CurrentEvent();
                if (storedEvent.OccuredOnUtc < Rev_1506)
                {
                    var rev1387 = Deserialize<Locked_Rev_883>(storedEvent);
                    current.OrganizationGuid = Guid.Empty;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = rev1387.OrganizationId;
                    current.UserIntegralId = rev1387.UserId;
                }
                else
                {
                    var rev1506 = Deserialize<Locked_Rev_1416>(storedEvent);
                    current.OrganizationGuid = rev1506.OrganizationId;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = 0;
                    current.UserIntegralId = rev1506.UserId;
                }


                if ((current.OrganizationGuid != Guid.Empty) && (current.UserGuid != Guid.Empty))
                {
                    return;
                }

                if (current.UserGuid == Guid.Empty)
                    current.UserGuid = GetUserGuid(current.UserIntegralId);
                if (current.OrganizationGuid == Guid.Empty)
                    current.OrganizationGuid = GetOrganizationGuid(current.OrganizationIntegralId);

                if (current.UserGuid == Guid.Empty)
                    throw new Exception(String.Format("Current.UserGuid is empty. Current.UserIntegralId = {0}.", current.UserIntegralId));
                if (current.OrganizationGuid == Guid.Empty)
                    throw new Exception(String.Format("Current.OrganizationGuid is empty. Current.OrganizationIntegralId = {0}", current.OrganizationIntegralId));
                UpdateSerialization(storedEvent.EventId, current);

                //if (OrganizationIdMap.ContainsKey(current.OrganizationIntegralId))
                //    return;

                //_sb.AppendLine(String.Format("Org: {0}, {1}, User {2}", current.OrganizationIntegralId, current.OrganizationGuid,
                //    current.UserIntegralId));
            });
        }

        private void ProcessUnlocked(string typeName)
        {
            ForEach(typeName, storedEvent =>
            {
                var current = new CurrentEvent();
                if (storedEvent.OccuredOnUtc < Rev_1506)
                {
                    var rev1387 = Deserialize<Unlocked_Rev_883>(storedEvent);
                    current.OrganizationGuid = Guid.Empty;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = rev1387.OrganizationId;
                    current.UserIntegralId = rev1387.UserId;
                }
                else
                {
                    var rev1506 = Deserialize<Unlocked_Rev_1416>(storedEvent);
                    current.OrganizationGuid = rev1506.OrganizationId;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = 0;
                    current.UserIntegralId = rev1506.UserId;
                }


                if ((current.OrganizationGuid != Guid.Empty) && (current.UserGuid != Guid.Empty))
                {
                    return;
                }

                if (current.UserGuid == Guid.Empty)
                    current.UserGuid = GetUserGuid(current.UserIntegralId);
                if (current.OrganizationGuid == Guid.Empty)
                    current.OrganizationGuid = GetOrganizationGuid(current.OrganizationIntegralId);

                if (current.UserGuid == Guid.Empty)
                    throw new Exception(String.Format("Current.UserGuid is empty. Current.UserIntegralId = {0}.", current.UserIntegralId));
                if (current.OrganizationGuid == Guid.Empty)
                    throw new Exception(String.Format("Current.OrganizationGuid is empty. Current.OrganizationIntegralId = {0}", current.OrganizationIntegralId));
                UpdateSerialization(storedEvent.EventId, current);

                //if (OrganizationIdMap.ContainsKey(current.OrganizationIntegralId))
                //    return;

                //_sb.AppendLine(String.Format("Org: {0}, {1}, User {2}", current.OrganizationIntegralId, current.OrganizationGuid,
                //    current.UserIntegralId));
            });
        }

        [DataContract]
        public class CurrentEvent : MigratingDomainEvent
        {
            [DataMember(Order = 3)]
            public Guid OrganizationGuid { get; set; }

            [DataMember(Order = 4)]
            public Guid UserGuid { get; set; }

            [DataMember(Order = 1)]
            public int OrganizationIntegralId { get; set; }

            [DataMember(Order = 2)]
            public int UserIntegralId { get; set; }
        }

        [DataContract]
        public class Locked_Rev_883 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrganizationId { get; set; }

            [DataMember(Order = 2)]
            public int UserId { get; set; }
        }

        [DataContract]
        public class Locked_Rev_1416 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 2)]
            public int UserId { get; set; }
        }

        [DataContract]
        public class Unlocked_Rev_883 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrganizationId { get; set; }

            [DataMember(Order = 2)]
            public int UserId { get; set; }
        }

        [DataContract]
        public class Unlocked_Rev_1416 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Guid OrganizationId { get; set; }

            [DataMember(Order = 2)]
            public int UserId { get; set; }
        }
    }
}