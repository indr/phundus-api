using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Web.ViewModels.Layout
{
    public class NavBarModel
    {
        private IList<Organization> _organizations = new List<Organization>();

        public IList<Organization> Organizations
        {
            get { return _organizations; }
            set { _organizations = value; }
        }

        public Organization Selected { get; set; }
        public OrganizationMembership Membership { get; set; }
    }
}