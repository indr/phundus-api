namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Cqrs;
    using Ddd;
    using Model;
    using Queries;
    using Repositories;

    public class UpdateOrganizationDetails
    {
        public Guid OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string Coordinate { get; set; }
        public string Startpage { get; set; }
        public string DocumentTemplate { get; set; }
    }

    public class UpdateOrganizationDetailsHandler : IHandleCommand<UpdateOrganizationDetails>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrganizationRepository Repository { get; set; }

        public void Handle(UpdateOrganizationDetails command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var organization = Repository.GetById(command.OrganizationId);

            organization.Address = command.Address;
            organization.EmailAddress = command.EmailAddress;
            organization.Website = command.Website;
            organization.Coordinate = command.Coordinate;
            organization.Startpage = command.Startpage;
            organization.DocTemplateFileName = command.DocumentTemplate;

            EventPublisher.Publish(new OrganizationUpdated());
        }
    }
}