namespace phiNdus.fundus.Web.Controllers.WebApi.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Phundus.Core.Services;

    public class MembershipApplicationsController : ApiController
    {
        public IOrganizationService OrganizationService { get; set; }

        public void Post(int orgId, MembershipApplicationDoc dto)
        {
            OrganizationService.CreateMembershipApplication(orgId, dto.UserId);
        }
    }

    public class MembershipApplicationDoc
    {
        private DateTime _createdOn = DateTime.UtcNow;

        public int OrgId { get; set; }
        
        public int UserId { get; set; }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
    }
}