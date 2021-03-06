﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Organizations;
    using Model.Users;

    public class ApplyForMembership : ICommand
    {
        public ApplyForMembership(InitiatorId initiatorId, MembershipApplicationId applicationId, UserId applicantId,
            OrganizationId organizationId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (applicationId == null) throw new ArgumentNullException("applicationId");
            if (applicantId == null) throw new ArgumentNullException("applicantId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            InitiatorId = initiatorId;
            ApplicationId = applicationId;
            ApplicantId = applicantId;
            OrganizationId = organizationId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public MembershipApplicationId ApplicationId { get; protected set; }
        public UserId ApplicantId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
    }

    public class ApplyForMembershipHandler : IHandleCommand<ApplyForMembership>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMembershipRequestRepository RequestRepository { get; set; }

        [Transaction]
        public void Handle(ApplyForMembership command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var user = UserRepository.GetById(command.ApplicantId);

            var request = organization.ApplyForMembership(command.InitiatorId,
                command.ApplicationId, user);

            RequestRepository.Add(request);
        }
    }
}