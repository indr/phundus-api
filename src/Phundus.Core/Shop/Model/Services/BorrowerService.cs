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
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessee GetById(Guid id);

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
        public IUserQueries UserQueries { get; set; }

        public Lessee GetById(Guid id)
        {
            var user = UserQueries.GetByGuid(new UserId(id));
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", id));

            return ToBorrower(user);
        }

        public Lessee GetById(LesseeId lesseeId)
        {
            return GetById(lesseeId.Id);
        }

        private static Lessee ToBorrower(IUser user)
        {
            return new Lessee(new LesseeId(user.UserId), user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.EmailAddress, user.MobilePhone, user.JsNummer.ToString());
        }
    }
}