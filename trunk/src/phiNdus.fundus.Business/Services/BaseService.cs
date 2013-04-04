using System;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.Services
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class BaseService
    {
        private SecurityContext _securityContext;
        private bool _sessionSet;

        public SecurityContext SecurityContext
        {
            get { return _securityContext; }
            set
            {
                Guard.Against<InvalidOperationException>(_sessionSet, "SecurityContext can only be set once");
                _sessionSet = true;
                _securityContext = value;
            }
        }

        public Organization SelectedOrganization
        {
            get { return SecurityContext.SecuritySession.User.SelectedOrganization; }
        }
    }
}