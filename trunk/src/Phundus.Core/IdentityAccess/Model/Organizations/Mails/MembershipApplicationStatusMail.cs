namespace Phundus.IdentityAccess.Model.Organizations.Mails
{
    using System;
    using System.Linq;
    using Application;
    using Common.Eventing;
    using Common.Mailing;
    using IdentityAccess.Organizations.Model;

    public class MembershipApplicationStatusMail :
        ISubscribeTo<MembershipApplicationApproved>,
        ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationRejected>
    {
        private readonly IMessageFactory _factory;
        private readonly IMailGateway _gateway;
        private readonly IMemberQueryService _memberQueryService;
        private readonly IOrganizationQueryService _organizationQueryService;
        private readonly IUserQueryService _userQueryService;

        public MembershipApplicationStatusMail(IMessageFactory factory, IMailGateway gateway,
            IOrganizationQueryService organizationQueryService, IUserQueryService userQueryService,
            IMemberQueryService memberQueryService)
        {
            _factory = factory;
            _gateway = gateway;
            _organizationQueryService = organizationQueryService;
            _userQueryService = userQueryService;
            _memberQueryService = memberQueryService;
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
            var recipients = _memberQueryService.Managers(e.OrganizationGuid, true).Select(x => x.EmailAddress).ToList();

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
            var organization = _organizationQueryService.GetById(organizationId);
            var user = _userQueryService.GetById(userId);

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