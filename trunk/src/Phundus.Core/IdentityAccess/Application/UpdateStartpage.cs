namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Projections;
    using Resources;
    using Users.Services;

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
        private readonly IUserInRole _userInRole;
        private readonly IMemberInRole _memberInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public UpdateStartpageHandler(IUserInRole userInRole, IMemberInRole memberInRole, IOrganizationRepository organizationRepository)
        {
            _userInRole = userInRole;
            _memberInRole = memberInRole;
            _organizationRepository = organizationRepository;
        }
                             
        [Transaction]
        public void Handle(UpdateStartpage command)
        {
            // TODO: Manager
            var initiator = _userInRole.GetById(command.InitiatorId);
            _memberInRole.ActiveManager(command.OrganizationId.Id, command.InitiatorId);

            var organization = _organizationRepository.GetById(command.OrganizationId.Id);

            organization.ChangeStartpage(initiator, command.Startpage);
        }
    }
}