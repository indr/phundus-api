namespace phiNdus.fundus.Web.ViewModels.Layout
{
    using System.Collections.Generic;
    using Phundus.Core.OrganizationAndMembershipCtx.Queries;

    public class NavBarModel
    {
        private IList<OrganizationDto> _organizations = new List<OrganizationDto>();

        public IList<OrganizationDto> Organizations
        {
            get { return _organizations; }
            set { _organizations = value; }
        }

        public OrganizationDto Selected { get; set; }
    }
}