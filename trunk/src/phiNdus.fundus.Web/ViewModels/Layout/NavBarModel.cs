namespace phiNdus.fundus.Web.ViewModels.Layout
{
    using System.Collections.Generic;
    using Phundus.Core.OrganizationAndMembershipCtx.Queries;

    public class NavBarModel
    {
        private IList<MembershipDto> _memberships = new List<MembershipDto>();

        public MembershipDto Selected { get; set; }

        public IList<MembershipDto> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }
    }
}