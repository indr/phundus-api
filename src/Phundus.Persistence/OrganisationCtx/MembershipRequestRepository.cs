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

    public class OrganisationRepository : RepositoryBase<Organisation>, IOrganisationRepository
    {
        private IQueryOver<Organisation, Organisation> Organisations
        {
            get { return Session.QueryOver<Organisation>(); }
        }

        public Organisation ById(int id)
        {
            return Organisations.Where(p => p.Id == id).SingleOrDefault();
        }
    }
}