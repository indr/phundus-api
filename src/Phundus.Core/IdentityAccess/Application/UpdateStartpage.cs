﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Organizations;
    using Resources;

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
        private readonly IMemberInRole _memberInRole;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRole _userInRole;

        public UpdateStartpageHandler(IUserInRole userInRole, IMemberInRole memberInRole,
            IOrganizationRepository organizationRepository)
        {
            _userInRole = userInRole;
            _memberInRole = memberInRole;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(UpdateStartpage command)
        {
            var manager = _userInRole.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId.Id);
            organization.ChangeStartpage(manager, command.Startpage);
        }
    }
}