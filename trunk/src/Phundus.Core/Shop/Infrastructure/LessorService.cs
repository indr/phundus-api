namespace Phundus.Shop.Infrastructure
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Resources;
    using Model;

    public class LessorService : ILessorService
    {
        private readonly IOrganizationsResource _organizationsResource;
        private readonly IUsersResource _usersResource;

        public LessorService(IOrganizationsResource organizationsResource, IUsersResource usersResource)
        {
            _organizationsResource = organizationsResource;
            _usersResource = usersResource;
        }

        public Lessor GetById(LessorId lessorId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");

            var organization = _organizationsResource.Get(lessorId.Id);
            if (organization != null)
                return ToLessor(organization);

            var user = _usersResource.Get(lessorId.Id);
            if (user != null)
                return ToLessor(user);

            throw new NotFoundException(String.Format("Lessor {0} not found.", lessorId));
        }

        private static Lessor ToLessor(UserData user)
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