namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using IdentityAccess.Model.Users;

    [DataContract]
    public class MemberRecieveEmailNotificationOptionChanged : DomainEvent
    {
        public MemberRecieveEmailNotificationOptionChanged(Manager manager, OrganizationId organizationId,
            UserId memberId,
            bool value)
        {
            if (manager == null) throw new ArgumentNullException("manager");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (memberId == null) throw new ArgumentNullException("memberId");

            Initiator = new Initiator(new InitiatorId(manager.UserId), manager.EmailAddress, manager.FullName);
            OrganizationId = organizationId.Id;
            MemberId = memberId.Id;
            Value = value;
        }

        protected MemberRecieveEmailNotificationOptionChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid MemberId { get; protected set; }

        [DataMember(Order = 4)]
        public bool Value { get; protected set; }
    }
}