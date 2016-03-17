namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Domain.Model;
    using Common.Querying;
    using NHibernate;

    public interface IUserAddressQueryService
    {
        UserAddressData FindById(InitiatorId initiatorId, Guid userId);
    }

    public class UserAddressQueryService : QueryServiceBase<UserAddressData>, IUserAddressQueryService
    {
        public UserAddressData FindById(InitiatorId initiatorId, Guid userId)
        {
            if (initiatorId == null)
                return null;

            var query = QueryOver();
            AuthFilter(query, initiatorId);
            query.Where(p => p.UserId == userId);

            return query.SingleOrDefault();
        }

        private static void AuthFilter(IQueryOver<UserAddressData, UserAddressData> query, InitiatorId initiatorId)
        {
            query.Where(p => p.UserId == initiatorId.Id);
        }
    }

    public class UserAddressData
    {
        public virtual Guid UserId { get; set; }
        public virtual int UserShortId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
    }
}