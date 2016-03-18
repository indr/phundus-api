namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Users;

    [DataContract]
    public class PdfTemplateChanged : DomainEvent
    {
        public PdfTemplateChanged(Manager initiator, OrganizationId organizationId, string pdfTemplateFileName)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            Initiator = initiator.ToActor();
            OrganizationId = organizationId.Id;
            PdfTemplateFileName = pdfTemplateFileName;
        }

        protected PdfTemplateChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 3)]
        public string PdfTemplateFileName { get; protected set; }
    }
}