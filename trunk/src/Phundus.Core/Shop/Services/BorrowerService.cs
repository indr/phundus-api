namespace Phundus.Core.Shop.Services
{
    using Common;
    using Common.Domain.Model;
    using Contracts.Model;
    using IdentityAndAccess.Queries;    

    public interface IBorrowerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Borrower GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lesseeId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Borrower GetById(LesseeId lesseeId);
    }

    public class BorrowerService : IBorrowerService
    {
        public IUserQueries UserQueries { get; set; }

        public Borrower GetById(int id)
        {
            var user = UserQueries.GetById(id);
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", id));

            return ToBorrower(user);
        }

        public Borrower GetById(LesseeId lesseeId)
        {
            return GetById(lesseeId.Id);
        }

        private static Borrower ToBorrower(UserDto user)
        {
            return new Borrower(user.Id, user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.Email, user.MobilePhone, user.JsNumber.ToString());
        }
    }
}