namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Organizations.Model;
    using Projections;

    public class ChangeOrganizationContactDetails : ICommand
    {
        public ChangeOrganizationContactDetails(InitiatorId initiatorId, OrganizationId organizationId, string line1,
            string line2, string street, string postcode, string city, string phoneNumber, string emailAddress,
            string website)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Line1 = line1;
            Line2 = line2;
            Street = street;
            Postcode = postcode;
            City = city;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Website = website;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string Line1 { get; protected set; }
        public string Line2 { get; protected set; }
        public string Street { get; protected set; }
        public string Postcode { get; protected set; }
        public string City { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string Website { get; protected set; }
    }

    public class ChangeOrganizationContactDetailsHandler : IHandleCommand<ChangeOrganizationContactDetails>
    {
        public IInitiatorService InitiatorService { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public IOrganizationRepository Repository { get; set; }

        [Transaction]
        public void Handle(ChangeOrganizationContactDetails command)
        {
            var initiator = InitiatorService.GetById(command.InitiatorId);
            MemberInRole.ActiveManager(command.OrganizationId.Id, command.InitiatorId);

            var organization = Repository.GetById(command.OrganizationId);

            organization.ChangeContactDetails(initiator, new ContactDetails(command.Line1, command.Line2, command.Street,
                command.Postcode, command.City, command.PhoneNumber, command.EmailAddress, command.Website));            
        }
    }
}