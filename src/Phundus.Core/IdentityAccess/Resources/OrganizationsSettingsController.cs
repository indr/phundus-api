namespace Phundus.IdentityAccess.Resources
{
    using System;
    using System.Net.Http;
    using Application;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Common.Resources;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/settings")]
    public class OrganizationsSettingsController : ApiControllerBase
    {
        private readonly IOrganizationQueryService _organizationQueryService;

        public OrganizationsSettingsController(IOrganizationQueryService organizationQueryService)
        {
            _organizationQueryService = organizationQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsSettingsGetOkResponseContent Get(Guid organizationId)
        {
            var organization = _organizationQueryService.GetById(organizationId);

            return new OrganizationsSettingsGetOkResponseContent
            {
                OrganizationId = organization.OrganizationId,
                Name = organization.Name,
                PublicRental = organization.PublicRental,
                PdfTemplate = organization.PdfTemplateFileName,
                OrderReceivedEmail = organization.OrderReceivedText
            };
        }

        [PATCH("")]
        public virtual HttpResponseMessage Patch(Guid organizationId, OrganizationsSettingsPatchRequestContent rq)
        {
            if(!String.IsNullOrWhiteSpace(rq.Name))
            {
                Dispatch(new RenameOrganization(CurrentUserId, new OrganizationId(organizationId), rq.Name));
            }
            if (rq.PublicRental.HasValue)
            {
                Dispatch(new ChangeSettingPublicRental(CurrentUserId, new OrganizationId(organizationId),
                    rq.PublicRental.Value));
            }
            if (rq.PdfTemplate != null)
            {
                Dispatch(new SetPdfTemplateFileName(CurrentUserId, new OrganizationId(organizationId),
                    rq.PdfTemplate));
            }
            if (rq.OrderReceivedEmail != null)
            {
                Dispatch(new ChangeEmailTemplate(CurrentUserId, new OrganizationId(organizationId),
                    rq.OrderReceivedEmail));
            }
            return NoContent();
        }
    }

    public class OrganizationsSettingsPatchRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publicRental")]
        public bool? PublicRental { get; set; }

        [JsonProperty("pdfTemplate")]
        public string PdfTemplate { get; set; }

        [JsonProperty("orderReceivedEmail")]
        public string OrderReceivedEmail { get; set; }
    }

    public class OrganizationsSettingsGetOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }

        [JsonProperty("pdfTemplate")]
        public string PdfTemplate { get; set; }

        [JsonProperty("orderReceivedEmail")]
        public string OrderReceivedEmail { get; set; }
    }
}