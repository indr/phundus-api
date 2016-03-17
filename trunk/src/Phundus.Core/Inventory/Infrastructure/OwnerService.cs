namespace Phundus.Inventory.Infrastructure
{
    using System;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Application;
    using IdentityAccess.Resources;
    using Model;
    using Model.Collaborators;

    public class OwnerService : IOwnerService
    {
        private readonly IOrganizationsResource _organizationsResource;
        private readonly IUsersResource _usersResource;

        public OwnerService(IOrganizationsResource organizationsResource, IUsersResource usersResource)
        {
            _organizationsResource = organizationsResource;
            _usersResource = usersResource;
        }

        [Transaction]
        public Owner GetById(OwnerId ownerId)
        {
            Owner result = FindById(ownerId);
            if (result == null)
                throw new NotFoundException(String.Format("Owner {0} not found.", ownerId));

            return result;
        }

        [Transaction]
        public Owner FindById(OwnerId ownerId)
        {
            OrganizationData organization = _organizationsResource.Get(ownerId.Id);
            if (organization != null)
                return ToOwner(organization);

            UserData user = _usersResource.Get(ownerId.Id);
            if (user != null)
                return ToOwner(user);

            return null;
        }

        private static Owner ToOwner(OrganizationData organization)
        {
            return new Owner(new OwnerId(organization.OrganizationId), organization.Name, OwnerType.Organization);
        }

        private static Owner ToOwner(UserData user)
        {
            return new Owner(new OwnerId(user.UserId), user.FirstName + " " + user.LastName, OwnerType.User);
        }
    }
}