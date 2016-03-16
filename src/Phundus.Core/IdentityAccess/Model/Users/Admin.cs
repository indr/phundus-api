namespace Phundus.IdentityAccess.Model.Users
{
    using Common.Domain.Model;

    public class Admin : Actor
    {
        public Admin(UserId userId, string emailAddress, string fullName) : base(userId.Id, emailAddress, fullName)
        {
        }

        protected Admin()
        {
        }

        public UserId UserId
        {
            get { return new UserId(ActorGuid); }
        }
    }
}