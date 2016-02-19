namespace Phundus.Shop.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model;

    public interface ILesseeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lesseeId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessee GetById(LesseeId lesseeId);
    }

    public class LesseeService : ILesseeService
    {
        private readonly IUserQueries _userQueries;

        public LesseeService(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            _userQueries = userQueries;
        }

        public Lessee GetById(LesseeId lesseeId)
        {
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var user = _userQueries.GetByGuid(new UserId(lesseeId.Id));
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", lesseeId));

            return ToLessee(user);
        }

        private static Lessee ToLessee(IUser user)
        {
            return new Lessee(new LesseeId(user.UserId), user.FirstName, user.LastName, user.Street, user.Postcode,
                user.City, user.EmailAddress, user.MobilePhone, user.JsNummer.ToString());
        }
    }
}