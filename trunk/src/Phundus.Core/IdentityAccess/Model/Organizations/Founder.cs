namespace Phundus.IdentityAccess.Model.Organizations
{
    using Common.Domain.Model;

    public class Founder : Actor
    {
        public Founder(UserId userId, string emailAddress, string fullName) : base(userId.Id, emailAddress, fullName)
        {
        }

        public UserId UserId
        {
            get { return new UserId(ActorGuid); }
        }
    }
}