namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Queries;
    using Repositories;

    public class UpdateStartpage
    {
        public UpdateStartpage(UserGuid initiatorId, Guid organizationId, string startpage)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Startpage = startpage;
        }

        public Guid OrganizationId { get; protected set; }
        public UserGuid InitiatorId { get; protected set; }
        public string Startpage { get; protected set; }
    }

    public class UpdateStartpageHandler : IHandleCommand<UpdateStartpage>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrganizationRepository Repository { get; set; }

        public void Handle(UpdateStartpage command)
        {
            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var organization = Repository.GetById(command.OrganizationId);

            organization.ChangeStartpage(command.InitiatorId, command.Startpage);
        }
    }
}