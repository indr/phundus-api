namespace Phundus.Core.OrganizationAndMembershipCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using Model;
    using NHibernate;
    using Repositories;

    public interface IOrganizationQueries
    {
        IEnumerable<OrganizationDto> ByMemberId(int memberId);
        OrganizationDto ById(int id);
    }

    public class OrganizationsReadModel : IOrganizationQueries
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public Func<ISession> SessionFactory { get; set; }

        protected virtual ISession Session
        {
            get { return SessionFactory(); }
        }

        public IEnumerable<OrganizationDto> ByMemberId(int memberId)
        {
            Organization orgAlias = null;
            Membership memberAlias = null;
            var query = Session.QueryOver(() => orgAlias)
                .JoinAlias(() => orgAlias.Memberships, () => memberAlias)
                .Where(() => memberAlias.MemberId == memberId);

            var result = new List<OrganizationDto>();
            foreach (var each in query.List())
            {
                result.Add(new OrganizationDto
                {
                    Id = each.Id,
                    Name = each.Name,
                    Url = each.Url
                });
            }
            return result;
        }

        public OrganizationDto ById(int id)
        {
            var organization = OrganizationRepository.FindById(id);
            if (organization == null)
                return null;

            return new OrganizationDto
            {
                Id = organization.Id,
                Url = organization.Url
            };
        }
    }

    public class OrganizationDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
    }
}