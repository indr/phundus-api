namespace Phundus.Persistence.OrganisationCtx
{
    #region

    using System;
    using System.Collections.Generic;
    using Core.OrganisationCtx.DomainModel;
    using Core.OrganisationCtx.Repositories;
    using NHibernate;

    #endregion

    public class MembershipRequestRepository : RepositoryBase<MembershipRequest>, IMembershipRequestRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<MembershipRequest> ByOrganization(int organizationId)
        {
            return Entities.Where(p => p.OrganizationId == organizationId).List();
        }

        private IQueryOver<MembershipRequest, MembershipRequest> Entities
        {
            get { return Session.QueryOver<MembershipRequest>(); }
        }
    }
}