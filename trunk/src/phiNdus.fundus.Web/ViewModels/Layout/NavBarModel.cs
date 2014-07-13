using System.Collections.Generic;

namespace phiNdus.fundus.Web.ViewModels.Layout
{
    using Phundus.Core.Entities;

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