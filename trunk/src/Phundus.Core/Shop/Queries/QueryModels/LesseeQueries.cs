namespace Phundus.Shop.Queries.QueryModels
{
    using System;
    using Common;
    using Cqrs;
    using Integration.IdentityAccess;
    using Integration.Shop;

    public class LesseeQueries : ReadModelBase, ILesseeQueries
    {
        private readonly IUserQueries _userQueries;

        public LesseeQueries(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            _userQueries = userQueries;
        }

        public ILessee GetByGuid(Guid lesseeGuid)
        {
            var user = _userQueries.FindById(lesseeGuid);
            if (user == null)
                throw new NotFoundException("Lessee {0} not found", lesseeGuid);

            return new LesseeViewRow(user.UserGuid, user.FullName, user.Street + "\n" + user.Postcode + " " + user.City,
                user.MobilePhone, user.EmailAddress);
        }
    }
}