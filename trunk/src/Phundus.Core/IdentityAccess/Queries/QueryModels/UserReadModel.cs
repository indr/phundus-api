namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;

    public class UserReadModel : NHibernateReadModelBase<UserViewRow>, IUserQueries
    {
        public IUser GetByGuid(Guid guid)
        {
            return GetByGuid(new UserGuid(guid));
        }

        public IUser GetByGuid(UserGuid userGuid)
        {
            var result = FindByGuid(userGuid);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userGuid);
            return result;
        }

        public IUser FindById(Guid userGuid)
        {
            return QueryOver()
                .Where(p => p.UserGuid == userGuid)
                .List().SingleOrDefault();
        }

        public IUser FindByGuid(UserGuid userGuid)
        {
            return FindById(userGuid.Id);
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