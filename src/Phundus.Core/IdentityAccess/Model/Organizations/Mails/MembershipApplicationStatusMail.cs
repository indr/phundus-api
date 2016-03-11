﻿namespace Phundus.IdentityAccess.Model.Organizations.Mails
{
    using System;
    using System.Linq;
    using Common.Mailing;
    using Common.Notifications;
    using IdentityAccess.Organizations.Model;
    using Integration.IdentityAccess;
    using Projections;

    public class MembershipApplicationStatusMail : IConsumes<MembershipApplicationFiled>,
        IConsumes<MembershipApplicationApproved>, IConsumes<MembershipApplicationRejected>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUsersQueries _usersQueries;
        private readonly IMembersWithRole _memberWithRole;

        public MembershipApplicationStatusMail(IMessageFactory factory, IMailGateway gateway,
            IOrganizationQueries organizationQueries, IUsersQueries usersQueries, IMembersWithRole memberWithRole)
        {
            _factory = factory;
            _gateway = gateway;
            _organizationQueries = organizationQueries;
            _usersQueries = usersQueries;
            _memberWithRole = memberWithRole;
        }

        public void Handle(MembershipApplicationApproved e)
        {
            var model = CreateModel(e.OrganizationGuid, e.UserGuid);

            var message = _factory.MakeMessage(model, Templates.MembershipApplicationApprovedSubject, null,
                Templates.MembershipApplicationApprovedHtml);
            message.To.Add(model.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public void Handle(MembershipApplicationFiled e)
        {
            var model = CreateModel(e.OrganizationGuid, e.UserGuid);
            var recipients = _memberWithRole.Manager(e.OrganizationGuid, true).Select(x => x.EmailAddress).ToList();

            var message = _factory.MakeMessage(model, Templates.MembershipApplicationFiledSubject, null,
                Templates.MembershipApplicationFiledHtml);
            message.To.Add(String.Join(",", recipients));

            _gateway.Send(e.OccuredOnUtc, message);
        }

        public void Handle(MembershipApplicationRejected e)
        {
            var model = CreateModel(e.OrganizationGuid, e.UserGuid);

            var message = _factory.MakeMessage(model, Templates.MembershipApplicationRejectedSubject, null,
                Templates.MembershipApplicationRejectedHtml);
            message.To.Add(model.EmailAddress);

            _gateway.Send(e.OccuredOnUtc, message);
        }

        private Model CreateModel(Guid organizationId, Guid userId)
        {
            var organization = _organizationQueries.GetById(organizationId);
            var user = _usersQueries.GetById(userId);

            return new Model
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                OrganizationName = organization.Name
            };
        }

        public class Model : MailModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }

            public string OrganizationName { get; set; }
        }
    }
}