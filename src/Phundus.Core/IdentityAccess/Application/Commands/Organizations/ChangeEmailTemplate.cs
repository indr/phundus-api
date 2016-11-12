namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Machine.Specifications.Annotations;
    using Model;
    using Model.Organizations;

    public class ChangeEmailTemplate : ICommand
    {
        public ChangeEmailTemplate([NotNull] InitiatorId initiatorId, [NotNull] OrganizationId organizationId,
            [NotNull] string orderReceivedText)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (orderReceivedText == null) throw new ArgumentNullException("orderReceivedText");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            OrderReceivedText = orderReceivedText;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string OrderReceivedText { get; protected set; }
    }

    public class ChangeEmailTemplateHandler : IHandleCommand<ChangeEmailTemplate>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRoleService _userInRoleService;

        public ChangeEmailTemplateHandler(IUserInRoleService userInRoleService,
            IOrganizationRepository organizationRepository)
        {
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(ChangeEmailTemplate command)
        {
            var manager = _userInRoleService.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.ChangeEmailTemplate(manager, command.OrderReceivedText);
        }
    }
}