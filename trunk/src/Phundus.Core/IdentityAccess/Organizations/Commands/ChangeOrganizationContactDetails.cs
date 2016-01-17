namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
    using Queries;
    using Repositories;

    public class ChangeOrganizationContactDetails : ICommand
    {
        public ChangeOrganizationContactDetails(InitiatorGuid initiatorId, OrganizationGuid organizationId, string postAddress,
            string phoneNumber, string emailAddress, string website)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            PostAddress = postAddress;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Website = website;
        }

        public InitiatorGuid InitiatorId { get; protected set; }
        public OrganizationGuid OrganizationId { get; protected set; }
        public string PostAddress { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string Website { get; protected set; }
    }

    public class ChangeOrganizationContactDetailsHandler : IHandleCommand<ChangeOrganizationContactDetails>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrganizationRepository Repository { get; set; }

        public void Handle(ChangeOrganizationContactDetails command)
        {
            MemberInRole.ActiveChief(command.OrganizationId.Id, command.InitiatorId);

            var organization = Repository.GetById(command.OrganizationId.Id);

            organization.ChangeContactDetails(new ContactDetails(command.PostAddress, command.PhoneNumber,
                command.EmailAddress, command.Website));

            EventPublisher.Publish(new OrganizationUpdated());
        }
    }
}