namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using NHibernate;

    public class UserReadModel : NHibernateReadModelBase<UserViewRow>, IUserQueries, IInitiatorService
    {
        public IUser GetByGuid(Guid guid)
        {
            return GetByGuid(new UserId(guid));
        }

        public IUser GetByGuid(UserId userId)
        {
            var result = FindByGuid(userId);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userId);
            return result;
        }

        public IUser FindById(Guid userGuid)
        {
            return QueryOver()
                .Where(p => p.UserId == userGuid)
                .List().SingleOrDefault();
        }

        public IUser FindByGuid(UserId userId)
        {
            return FindById(userId.Id);
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
                .Where(p => p.UserId == userGuid)
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

        public Initiator GetActiveById(InitiatorId initiatorId)
        {
            var user = GetByGuid(initiatorId.Id);
            return new Initiator(new InitiatorId(user.UserId), user.EmailAddress, user.FullName);
        }
    }
}