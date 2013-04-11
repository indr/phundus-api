namespace phiNdus.fundus.Web.Business.Services
{
    using phiNdus.fundus.Business.Security;
    using phiNdus.fundus.Domain.Entities;

    public class BaseService
    {
        private SecurityContext SecurityContext
        {
            get { return null; }
        }

        public Organization SelectedOrganization
        {
            get { return SecurityContext.SecuritySession.User.SelectedOrganization; }
        }
    }
}