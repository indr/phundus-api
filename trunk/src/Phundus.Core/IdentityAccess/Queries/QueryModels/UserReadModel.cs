﻿namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Integration.IdentityAccess;

    public class UserReadModel : NHibernateReadModelBase<UserViewRow>, IUserQueries
    {
        public IUser GetById(int id)
        {
            return GetById(new UserId(id));
        }

        public IUser GetById(UserId userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            var result = QueryOver()
                .Where(p => p.UserId == userId.Id)
                .List().SingleOrDefault();
            if (result == null)
                throw new NotFoundException("User {0} not found.", userId);
            return result;
        }

        public IUser FindById(int userId)
        {
            return QueryOver()
                .Where(p => p.UserId == userId)
                .List().SingleOrDefault();
        }

        public IUser FindById(Guid userGuid)
        {
            return QueryOver()
                .Where(p => p.UserGuid == userGuid)
                .List().SingleOrDefault();
        }

        public IUser FindByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            username = username.ToLowerInvariant().Trim();

            return QueryOver()
                .Where(p => p.EmailAddress == username)
                .List().SingleOrDefault();
        }

        public IList<IUser> Query()
        {
            return QueryOver()
                .List<IUser>();
        }

        public IUser FindActiveById(Guid userGuid)
        {
            return QueryOver()
                .Where(p => p.UserGuid == userGuid)
                .And(p => p.IsApproved && !p.IsLockedOut)
                .List().SingleOrDefault();
        }

        public bool IsEmailAddressTaken(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            emailAddress = emailAddress.ToLowerInvariant().Trim();

            return QueryOver()
                .Where(p => p.EmailAddress == emailAddress)
                .List().SingleOrDefault() != null;
        }
    }
}