namespace Phundus.Inventory.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Queries;
    using IdentityAccess.Queries.ReadModels;
    using Integration.IdentityAccess;
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
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Owner GetById(OwnerId ownerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Owner GetByUserId(UserId userId);

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

            throw new NotFoundException(String.Format("Owner {0} not found.", ownerId));
        }

        public Owner GetById(OwnerId ownerId)
        {
            return GetById(ownerId.Id);
        }

        public Owner GetByUserId(UserId userId)
        {
            var user = _userQueries.GetByGuid(userId);
            if (user == null)
                throw new NotFoundException(String.Format("User {0} not found.", userId));

            return ToOwner(user);
        }

        public Owner GetByOrganizationId(Guid organizationId)
        {
            var organization = _organizationQueries.FindById(organizationId);
            if (organization == null)
                throw new NotFoundException(String.Format("Organization {0} not found.", organizationId));

            return ToOwner(organization);
        }

        private static Owner ToOwner(OrganizationDetailDto organization)
        {
            return new Owner(new OwnerId(organization.Guid), organization.Name);
        }

        private static Owner ToOwner(IUser user)
        {
            return new Owner(new OwnerId(user.UserId), user.FirstName + " " + user.LastName);
        }
    }
}