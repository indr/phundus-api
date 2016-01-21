namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Queries;
    using Repositories;

    public class UpdateStartpage
    {
        public UpdateStartpage(InitiatorId initiatorId, OrganizationId organizationId, string startpage)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Startpage = startpage;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string Startpage { get; protected set; }
    }

    public class UpdateStartpageHandler : IHandleCommand<UpdateStartpage>
    {
        private readonly IMemberInRole _memberInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateStartpageHandler(IMemberInRole memberInRole, IOrganizationRepository organizationRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _memberInRole = memberInRole;
            _organizationRepository = organizationRepository;
        }

        public void Handle(UpdateStartpage command)
        {
            _memberInRole.ActiveManager(command.OrganizationId.Id, command.InitiatorId);

            var organization = _organizationRepository.GetById(command.OrganizationId.Id);

            organization.ChangeStartpage(command.InitiatorId, command.Startpage);
        }
    }
}