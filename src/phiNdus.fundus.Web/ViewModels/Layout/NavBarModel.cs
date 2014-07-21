namespace phiNdus.fundus.Web.ViewModels.Layout
{
    using System.Collections.Generic;
    using Phundus.Core.OrganisationCtx;
    using Phundus.Core.OrganisationCtx.DomainModel;

    public class NavBarModel
    {
        private IList<Organization> _organizations = new List<Organization>();

        public IList<Organization> Organizations
        {
            get { return _organizations; }
            set { _organizations = value; }
        }

        public Organization Selected { get; set; }
        public Membership Membership { get; set; }
    }
}