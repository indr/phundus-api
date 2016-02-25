namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using Common;
    using Cqrs;
    using Integration.IdentityAccess;
    using Integration.Shop;

    public interface ILesseeQueries
    {
        ILessee GetByGuid(Guid lesseeId);
    }

    public class LesseeQueries : ProjectionBase, ILesseeQueries
    {
        private readonly IUserQueries _userQueries;

        public LesseeQueries(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            _userQueries = userQueries;
        }

        public ILessee GetByGuid(Guid lesseeId)
        {
            var user = _userQueries.FindById(lesseeId);
            if (user == null)
                throw new NotFoundException("Lessee {0} not found", lesseeId);

            return new LesseeViewRow(user.UserId, user.FullName, user.Street + "\n" + user.Postcode + " " + user.City,
                user.MobilePhone, user.EmailAddress);
        }
    }

    public class LesseeViewRow : ILessee
    {
        internal LesseeViewRow(Guid lesseeGuid, string name, string address, string phoneNumber, string emailAddress)
        {
            LesseeGuid = lesseeGuid;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Guid LesseeGuid { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
    }
}