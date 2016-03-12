﻿namespace Phundus.Inventory.Model.Collaborators
{
    using System;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;

    public interface IOwnerService
    {
        Owner GetById(OwnerId ownerId);
        Owner FindById(OwnerId ownerId);
    }

    public class OwnerService : IOwnerService
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUsersQueries _usersQueries;

        public OwnerService(IOrganizationQueries organizationQueries, IUsersQueries usersQueries)
        {            
            _organizationQueries = organizationQueries;
            _usersQueries = usersQueries;
        }

        [Transaction]
        public Owner GetById(OwnerId ownerId)
        {
            var result = FindById(ownerId);   
            if (result == null)
                throw new NotFoundException(String.Format("Owner {0} not found.", ownerId));

            return result;
        }

        [Transaction]
        public Owner FindById(OwnerId ownerId)
        {
            var organization = _organizationQueries.FindById(ownerId.Id);
            if (organization != null)
                return ToOwner(organization);

            var user = _usersQueries.FindById(ownerId.Id);
            if (user != null)
                return ToOwner(user);

            return null;
        }

        private static Owner ToOwner(OrganizationData organization)
        {
            return new Owner(new OwnerId(organization.OrganizationId), organization.Name, OwnerType.Organization);
        }

        private static Owner ToOwner(IUser user)
        {
            return new Owner(new OwnerId(user.UserId), user.FirstName + " " + user.LastName, OwnerType.User);
        }
    }
}