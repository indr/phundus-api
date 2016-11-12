namespace Phundus.IdentityAccess.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Machine.Specifications.Annotations;
    using Users;

    [DataContract]
    public class EmailTemplateChanged : DomainEvent
    {
        public EmailTemplateChanged([NotNull] Manager initiator, [NotNull] OrganizationId organizationId,
            [NotNull] string orderReceivedText)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (orderReceivedText == null) throw new ArgumentNullException("orderReceivedText");

            Initiator = initiator.ToActor();
            OrganizationId = organizationId.Id;
            OrderReceivedText = orderReceivedText;
        }

        protected EmailTemplateChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string OrderReceivedText { get; protected set; }
    }
}