namespace Phundus.Shop.Projections
{
    using System;
    using Common;
    using Common.Projecting;
    using Cqrs;
    using Integration.IdentityAccess;

    public interface ILesseeQueries
    {
        LesseeViewRow GetByGuid(Guid lesseeId);
    }

    public class LesseesProjection : ProjectionBase, ILesseeQueries
    {
        private readonly IUsersQueries _usersQueries;

        public LesseesProjection(IUsersQueries usersQueries)
        {
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");
            _usersQueries = usersQueries;
        }

        public LesseeViewRow GetByGuid(Guid lesseeId)
        {
            var user = _usersQueries.FindById(lesseeId);
            if (user == null)
                throw new NotFoundException("Lessee {0} not found", lesseeId);

            return new LesseeViewRow(user.UserId, user.FullName, user.Street + "\n" + user.Postcode + " " + user.City,
                user.MobilePhone, user.EmailAddress);
        }
    }

    public class LesseeViewRow
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