namespace Phundus.Shop.Infrastructure
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using IdentityAccess.Resources;
    using Model;

    public class LesseeService : ILesseeService
    {
        private readonly IUsersResource _usersQueries;

        public LesseeService(IUsersResource usersQueries)
        {
            _usersQueries = usersQueries;
        }

        public Lessee GetById(LesseeId lesseeId)
        {
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var user = _usersQueries.Get(lesseeId.Id);
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", lesseeId));

            return ToLessee(user);
        }

        private static Lessee ToLessee(UserData user)
        {
            return new Lessee(new LesseeId(user.UserId), user.FirstName, user.LastName, user.Street, user.Postcode,
                user.City, user.EmailAddress, user.MobilePhone, user.JsNummer.ToString());
        }
    }
}