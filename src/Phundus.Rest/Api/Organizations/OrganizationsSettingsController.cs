namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using IdentityAccess.Commands;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/settings")]
    public class OrganizationsSettingsController : ApiControllerBase
    {
        private readonly IOrganizationQueries _organizationQueries;

        public OrganizationsSettingsController(IOrganizationQueries organizationQueries)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            _organizationQueries = organizationQueries;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsSettingsGetOkResponseContent Get(Guid organizationId)
        {
            var organization = _organizationQueries.GetById(organizationId);

            return new OrganizationsSettingsGetOkResponseContent
            {
                OrganizationId = organization.OrganizationId,
                PublicRental = organization.PublicRental
            };
        }

        [PATCH("")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid organizationId,
            OrganizationsSettingsPatchRequestContent requestContent)
        {
            if (requestContent.PublicRental.HasValue)
            {
                Dispatch(new ChangeSettingPublicRental(CurrentUserId, new OrganizationId(organizationId),
                    requestContent.PublicRental.Value));
            }
            return NoContent();
        }
    }

    public class OrganizationsSettingsPatchRequestContent
    {
        [JsonProperty("publicRental")]
        public bool? PublicRental { get; set; }
    }

    public class OrganizationsSettingsGetOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }
    }
}