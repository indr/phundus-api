namespace Phundus.Core.Shop.Contracts.Services
{
    using IdentityAndAccess.Queries;
    using Model;

    public class BorrowerService : IBorrowerService
    {
        public IUserQueries UserQueries { get; set; }

        public Borrower ById(int id)
        {
            var user = UserQueries.ById(id);
            if (user == null)
                throw new BorrowerNotFoundException();

            return ToBorrower(user);
        }

        private static Borrower ToBorrower(UserDto user)
        {
            return new Borrower(user.Id, user.FirstName, user.LastName, user.Email);
        }
    }

    public interface IBorrowerService
    {
        Borrower ById(int id);
    }
}