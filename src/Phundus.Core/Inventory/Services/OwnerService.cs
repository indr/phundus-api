namespace Phundus.Core.Inventory.Services
{
    using System;
    using Common;
    using IdentityAndAccess.Queries;
    using Owners;

    public interface IOwnerService
    {
        Owner GetByUserId(int userId);
        Owner GetByOrganizationId(int organizationId);
    }

    public class OwnerService : IOwnerService
    {
        private readonly IUserQueries _userQueries;
        private readonly IOrganizationQueries _organizationQueries;

        public OwnerService(IOrganizationQueries organizationQueries, IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
        }

        public Owner GetByUserId(int userId)
        {
            var user =_userQueries.ById(userId);
            if (user == null)
                throw new InvalidOperationException(String.Format("User with id {0} not found.", userId));

            return ToOwner(user);
        }

        public Owner GetByOrganizationId(int organizationId)
        {
            var organization = _organizationQueries.ById(organizationId);
            if (organization == null)
                throw new InvalidOperationException(String.Format("Organization with id {0} not found.", organizationId));

            return ToOwner(organization);
        }

        private static Owner ToOwner(OrganizationDetailDto organization)
        {
            return new Owner(new OwnerId(organization.Guid), organization.Name);
        }

        private static Owner ToOwner(UserDto user)
        {
            return new Owner(new OwnerId(user.Guid), user.FirstName + " " + user.LastName);
        }
    }
}