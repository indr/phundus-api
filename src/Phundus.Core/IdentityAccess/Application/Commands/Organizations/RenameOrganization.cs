
namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Model;
    using Model.Organizations;
    using Phundus.Common.Commanding;

    public class RenameOrganization : ICommand
    {
        public RenameOrganization(InitiatorId initiatorId, OrganizationId organizationId, string name)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (name == null) throw new ArgumentNullException("name");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Name = name;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string Name { get; protected set; }
    }

    public class RenameOrganizationHandler : IHandleCommand<RenameOrganization>
    {
        private IUserInRoleService _userInRoleService;
        private IOrganizationRepository _organizationRepository;

        public RenameOrganizationHandler(IUserInRoleService userInRoleService, IOrganizationRepository organizationRepository)
        {
            if (userInRoleService == null) throw new ArgumentNullException("userInRoleService");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(RenameOrganization command)
        {
            var manager = _userInRoleService.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.Rename(manager, command.Name);
        }
    }
}
