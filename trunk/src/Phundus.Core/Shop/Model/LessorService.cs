namespace Phundus.Shop.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;

    public interface ILessorService
    {
        Lessor GetById(LessorId lessorId);
    }

    public class LessorService : ILessorService
    {        
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUsersQueries _usersQueries;

        public LessorService(IOrganizationQueries organizationQueries, IUsersQueries usersQueries)
        {
            _organizationQueries = organizationQueries;
            _usersQueries = usersQueries;            
        }

        public Lessor GetById(LessorId lessorId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");

            var organization = _organizationQueries.FindById(lessorId.Id);
            if (organization != null)
                return ToLessor(organization);

            var user = _usersQueries.FindById(lessorId.Id);
            if (user != null)
                return ToLessor(user);

            throw new NotFoundException(String.Format("Lessor {0} not found.", lessorId));
        }

        private static Lessor ToLessor(IUser user)
        {
            var lessorId = new LessorId(user.UserId);
            return new Lessor(lessorId, user.FullName, user.PostalAddress, user.MobilePhone, user.EmailAddress, null,
                true);
        }

        private static Lessor ToLessor(OrganizationData organization)
        {
            var lessorId = new LessorId(organization.OrganizationId);
            return new Lessor(lessorId, organization.Name, organization.PostalAddress, organization.PhoneNumber,
                organization.EmailAddress, organization.Website, organization.PublicRental);
        }
    }
}