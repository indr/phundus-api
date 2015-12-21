namespace phiNdus.fundus.Web.ViewModels.Layout
{
    using System.Collections.Generic;
    using Phundus.Core.IdentityAndAccess.Queries;

    public class NavBarModel
    {
        private IList<MembershipDto> _memberships = new List<MembershipDto>();        

        public IList<MembershipDto> Memberships
        {
            get { return _memberships; }
            set { _memberships = value; }
        }

        public string UserId { get; set; }
    }
}