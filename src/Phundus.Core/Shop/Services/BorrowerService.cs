namespace Phundus.Core.Shop.Services
{
    using Common;
    using Contracts.Model;
    using IdentityAndAccess.Queries;
    using Infrastructure;

    public interface IBorrowerService
    {
        Borrower ById(int id);
    }

    public class BorrowerService : IBorrowerService
    {
        public IUserQueries UserQueries { get; set; }

        public Borrower ById(int id)
        {
            var user = UserQueries.ById(id);
            if (user == null)
                throw new NotFoundException(string.Format("User with id {0} not found.", id));

            return ToBorrower(user);
        }

        private static Borrower ToBorrower(UserDto user)
        {
            return new Borrower(user.Id, user.FirstName, user.LastName, user.Street, user.Postcode, user.City,
                user.Email, user.MobilePhone, user.JsNumber.ToString());
        }
    }
}