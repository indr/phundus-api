namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Organizations;

    public class SetPdfTemplateFileName : ICommand
    {
        public SetPdfTemplateFileName(InitiatorId initiatorId, OrganizationId organizationId, string pdfTemplateFileName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (pdfTemplateFileName == null) throw new ArgumentNullException("pdfTemplateFileName");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            PdfTemplateFileName = pdfTemplateFileName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string PdfTemplateFileName { get; protected set; }
    }

    public class SetPdfTemplateFileNameHandler : IHandleCommand<SetPdfTemplateFileName>
    {
        private readonly IUserInRoleService _userInRoleService;
        private readonly IOrganizationRepository _organizationRepository;

        public SetPdfTemplateFileNameHandler(IUserInRoleService userInRoleService, IOrganizationRepository organizationRepository)
        {
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(SetPdfTemplateFileName command)
        {
            var manager = _userInRoleService.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.SetPdfTemplateFileName(manager, command.PdfTemplateFileName);
        }
    }
}