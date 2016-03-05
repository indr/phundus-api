namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Projections;

    public class UpdateStartpage : ICommand
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
        private readonly IInitiatorService _initiatorService;
        private readonly IMemberInRole _memberInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateStartpageHandler(IInitiatorService initiatorService, IMemberInRole memberInRole, IOrganizationRepository organizationRepository)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");

            _initiatorService = initiatorService;
            _memberInRole = memberInRole;
            _organizationRepository = organizationRepository;
        }
                             
        [Transaction]
        public void Handle(UpdateStartpage command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);
            _memberInRole.ActiveManager(command.OrganizationId.Id, command.InitiatorId);

            var organization = _organizationRepository.GetById(command.OrganizationId.Id);

            organization.ChangeStartpage(initiator, command.Startpage);
        }
    }
}