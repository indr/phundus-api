namespace Phundus.Migrations
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201601182024)]
    public class M201601182024_Migrate_membership_application_events : EventMigrationBase
    {
        private StringBuilder _sb;

        private DateTime Rev_1387 = new DateTime(2015, 12, 26, 09, 34, 00);
        private DateTime Rev_1506 = new DateTime(2016, 01, 07, 09, 03, 00);

        protected override void Migrate()
        {
            _sb = new StringBuilder();

            ProcessFiled(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationFiled, Phundus.Core");
            ProcessApproved(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationApproved, Phundus.Core");
            ProcessRejected(@"Phundus.IdentityAccess.Organizations.Model.MembershipApplicationRejected, Phundus.Core");
            
            if (_sb.Length > 0)
                throw new Exception(_sb.ToString());
        }

        private void ProcessFiled(string typeName)
        {
            ForEach(typeName, storedEvent =>
            {
                var current = new CurrentMembershipApplicationEvent();
                if (storedEvent.OccuredOnUtc < Rev_1506)
                {
                    var rev1387 = Deserialize<Filed_Rev_1387>(storedEvent);
                    current.InitiatorId = Guid.Empty;
                    current.OrganizationGuid = Guid.Empty;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = rev1387.OrganizationId;
                    current.UserIntegralId = rev1387.UserId;
                }
                else
                {
                    var rev1506 = Deserialize<Filed_Rev_1506>(storedEvent);
                    current.InitiatorId = Guid.Empty;
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

        private void ProcessApproved(string typeName)
        {
            ForEach(typeName, storedEvent =>
            {
                var current = new CurrentMembershipApplicationEvent();

                if (storedEvent.OccuredOnUtc < Rev_1506)
                {
                    var rev1387 = Deserialize<Approved_Rev_1387>(storedEvent);
                    current.InitiatorId = Guid.Empty;
                    current.OrganizationGuid = Guid.Empty;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = rev1387.OrganizationId;
                    current.UserIntegralId = rev1387.UserId;
                }
                else
                {
                    var rev1506 = Deserialize<Approved_Rev_1506>(storedEvent);
                    current.InitiatorId = Guid.Empty;
                    current.OrganizationGuid = rev1506.OrganizationId;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = 0;
                    current.UserIntegralId = rev1506.UserId;
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

        private void ProcessRejected(string typeName)
        {
            ForEach(typeName, storedEvent =>
            {
                var current = new CurrentMembershipApplicationEvent();

                if (storedEvent.OccuredOnUtc < Rev_1506)
                {
                    var rev1387 = Deserialize<Rejected_Rev_1387>(storedEvent);
                    current.InitiatorId = Guid.Empty;
                    current.OrganizationGuid = Guid.Empty;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = rev1387.OrganizationId;
                    current.UserIntegralId = rev1387.UserId;
                }
                else
                {
                    var rev1506 = Deserialize<Rejected_Rev_1506>(storedEvent);
                    current.InitiatorId = Guid.Empty;
                    current.OrganizationGuid = rev1506.OrganizationId;
                    current.UserGuid = Guid.Empty;
                    current.OrganizationIntegralId = 0;
                    current.UserIntegralId = rev1506.UserId;
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
        internal class CurrentMembershipApplicationEvent : DomainEvent
        {
            [DataMember(Order = 4)]
            public Guid InitiatorId { get; set; }

            [DataMember(Order = 3)]
            public Guid OrganizationGuid { get; set; }

            [DataMember(Order = 5)]
            public Guid UserGuid { get; set; }

            [DataMember(Order = 2)]
            public int UserIntegralId { get; set; }

            [DataMember(Order = 1)]
            public int OrganizationIntegralId { get; set; }
        }

        [DataContract]
        public class Filed_Rev_1387 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }

        [DataContract]
        public class Filed_Rev_1506 : MigratingDomainEvent
        {
            [DataMember(Order = 3)]
            public Guid OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }

        [DataContract]
        public class Approved_Rev_1387 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }

        [DataContract]
        public class Approved_Rev_1506 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Guid OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }

        [DataContract]
        public class Rejected_Rev_1387 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public int OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }

        [DataContract]
        public class Rejected_Rev_1506 : MigratingDomainEvent
        {
            [DataMember(Order = 1)]
            public Guid OrganizationId { get; protected set; }

            [DataMember(Order = 2)]
            public int UserId { get; protected set; }
        }
    }
}