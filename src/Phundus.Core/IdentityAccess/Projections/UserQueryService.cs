namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Common.Querying;
    using Model.Users;

    public class UserQueryService : QueryServiceBase<UserData>, IUserQueryService
    {
        public UserData GetById(Guid userId)
        {
            return GetByGuid(new UserId(userId));
        }

        public UserData FindById(Guid userId)
        {
            return QueryOver()
                .Where(p => p.UserId == userId)
                .List().SingleOrDefault();
        }

        [Transaction]
        public UserData FindByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            username = username.ToLowerInvariant().Trim();

            return QueryOver()
                .Where(p => p.EmailAddress == username)
                .List().SingleOrDefault();
        }

        public IList<UserData> Query()
        {
            return QueryOver()
                .List<UserData>();
        }

        public UserData FindActiveById(Guid userId)
        {
            return QueryOver()
                .Where(p => p.UserId == userId)
                .And(p => p.IsApproved && !p.IsLockedOut)
                .List().SingleOrDefault();
        }

        [Transaction]
        public bool IsEmailAddressTaken(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            emailAddress = emailAddress.ToLowerInvariant().Trim();

            return QueryOver()
                .Where(p => p.EmailAddress == emailAddress)
                .List().SingleOrDefault() != null;
        }

        public UserData GetByGuid(UserId userId)
        {
            var result = FindByGuid(userId);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userId);
            return result;
        }

        public UserData FindByGuid(UserId userId)
        {
            return FindById(userId.Id);
        }
    }

    public class UserData
    {
        public virtual string Username
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

        public virtual string PostalAddress
        {
            get { return PostalAddressFactory.Make(FirstName, LastName, Street, Postcode, City); }
        }

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