namespace Phundus.Shop.Application
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Querying;
    using IdentityAccess.Resources;

    public interface ILesseeQueryService
    {
        LesseeData GetById(CurrentUserId currentUserId, Guid lesseeId);
    }

    public class LesseeQueryService : QueryServiceBase, ILesseeQueryService
    {
        private readonly IUsersResource _usersQueries;

        public LesseeQueryService(IUsersResource usersQueries)
        {
            _usersQueries = usersQueries;
        }

        public LesseeData GetById(CurrentUserId currentUserId, Guid lesseeId)
        {
            if (currentUserId.Id != lesseeId)
                throw new NotFoundException("Lessee {0} not found.", lesseeId);

            var user = _usersQueries.Get(lesseeId);
            if (user == null)
                throw new NotFoundException("Lessee {0} not found", lesseeId);

            return new LesseeData(user.UserId, user.FullName,
                user.FullName + "\n" + user.Street + "\n" + user.Postcode + " " + user.City,
                user.MobilePhone, user.EmailAddress);
        }
    }

    public class LesseeData
    {
        internal LesseeData(Guid lesseeId, string name, string postalAddress, string phoneNumber, string emailAddress)
        {
            LesseeId = lesseeId;
            Name = name;
            PostalAddress = postalAddress;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Guid LesseeId { get; private set; }
        public string Name { get; private set; }
        public string PostalAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
    }
}