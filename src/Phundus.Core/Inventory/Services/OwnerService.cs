namespace Phundus.Inventory.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using Integration.IdentityAccess;

    public interface IOwnerService
    {
        Owner GetById(OwnerId ownerId);
    }

    public class OwnerService : IOwnerService
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUserQueries _userQueries;

        public OwnerService(IOrganizationQueries organizationQueries, IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
        }

        public Owner GetById(OwnerId ownerId)
        {
            var organization = _organizationQueries.FindById(ownerId.Id);
            if (organization != null)
                return ToOwner(organization);

            var user = _userQueries.FindById(ownerId.Id);
            if (user != null)
                return ToOwner(user);

            throw new NotFoundException(String.Format("Owner {0} not found.", ownerId));
        }

        private static Owner ToOwner(IOrganization organization)
        {
            return new Owner(new OwnerId(organization.OrganizationId), organization.Name, OwnerType.Organization);
        }

        private static Owner ToOwner(IUser user)
        {
            return new Owner(new OwnerId(user.UserId), user.FirstName + " " + user.LastName, OwnerType.User);
        }
    }
}