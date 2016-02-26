namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;

    public class UsersProjection : ProjectionBase<UserData>, IUsersQueries, IInitiatorService
    {
        public Initiator GetActiveById(InitiatorId initiatorId)
        {
            var user = GetByGuid(initiatorId.Id);
            return new Initiator(new InitiatorId(user.UserId), user.EmailAddress, user.FullName);
        }

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

        public IUser FindByGuid(UserId userId)
        {
            return FindById(userId.Id);
        }
    }

    public class UserData : IUser
    {
        public virtual string Username
        {
            get { return EmailAddress; }
        }

        /// <summary>
        /// Für E-Mail-Template!
        /// </summary>
        public virtual string Email
        {
            get { return EmailAddress; }
        }

        public virtual DateTime? LastPasswordChangeAtUtc { get; set; }
        public virtual DateTime? LastLockOutAtUtc { get; set; }
        public virtual Guid UserId { get; set; }

        public virtual int RoleId { get; set; }

        public virtual string RoleName
        {
            get
            {
                if (RoleId == (int) UserRole.Admin)
                    return "Admin";
                if (RoleId == (int) UserRole.User)
                    return "User";
                return null;
            }
        }

        public virtual string EmailAddress { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual int? JsNummer { get; set; }

        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime SignedUpAtUtc { get; set; }
        public virtual DateTime? LastLogInAtUtc { get; set; }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}