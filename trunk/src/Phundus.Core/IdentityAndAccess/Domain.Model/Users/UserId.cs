namespace Phundus.Core.IdentityAndAccess.Domain.Model.Users
{
    using Common.Domain.Model;

    public class UserId : Identity
    {
        public UserId(string id) : base(id)
        {
        }
    }
}