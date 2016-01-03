namespace Phundus.Core.Shop.Services
{
    using Common;
    using Common.Domain.Model;
    using Contracts.Model;
    using IdentityAndAccess.Queries;    

    public interface ILesseeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessee GetById(int id);

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

        public Lessee GetById(int id)
        {
            var user = UserQueries.GetById(id);
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", id));

            return ToBorrower(user);
        }

        public Lessee GetById(LesseeId lesseeId)
        {
            return GetById(lesseeId.Id);
        }

        private static Lessee ToBorrower(UserDto user)
        {
            return new Lessee(user.Id, user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.Email, user.MobilePhone, user.JsNumber.ToString());
        }
    }
}