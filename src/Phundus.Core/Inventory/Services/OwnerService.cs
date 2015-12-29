namespace Phundus.Core.Inventory.Services
{
    using System;
    using Common;
    using IdentityAndAccess.Queries;
    using Owners;

    public interface IOwnerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Owner GetById(Guid ownerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Owner GetByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Owner GetByOrganizationId(Guid organizationId);
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

        public Owner GetById(Guid ownerId)
        {
            var organization = _organizationQueries.FindById(ownerId);
            if (organization != null)
                return ToOwner(organization);

            var user = _userQueries.FindById(ownerId);
            if (user != null)
                return ToOwner(user);

            throw new NotFoundException(String.Format("Owner with id {0} not found.", ownerId));
        }

        public Owner GetByUserId(int userId)
        {
            var user = _userQueries.GetById(userId);
            if (user == null)
                throw new NotFoundException(String.Format("User with id {0} not found.", userId));

            return ToOwner(user);
        }

        public Owner GetByOrganizationId(Guid organizationId)
        {
            var organization = _organizationQueries.FindById(organizationId);
            if (organization == null)
                throw new NotFoundException(String.Format("Organization with id {0} not found.", organizationId));

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