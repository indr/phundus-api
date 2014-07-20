namespace Phundus.Persistence.OrganisationCtx
{
    using System;
    using Core.OrganisationCtx.DomainModel;
    using Core.OrganisationCtx.Repositories;
    using NHibernate;

    public class MembershipRequestRepository : RepositoryBase<MembershipRequest>, IMembershipRequestRepository
    {
        public int NextIdentity()
        {
            throw new NotImplementedException();
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