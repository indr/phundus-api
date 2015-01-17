namespace Phundus.Core.IdentityAndAccess.Domain.Model.Users
{
    using Common.Domain.Model;

    public class UserId : Identity<int>
    {
        public UserId(int id) : base(id)
        {
        }

        public static UserId Root { get { return new UserId(1); } }
    }
}