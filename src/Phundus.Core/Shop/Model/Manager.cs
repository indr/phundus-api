namespace Phundus.Shop.Model
{
    using Common.Domain.Model;

    public class Manager : Actor
    {
        public Manager(UserId userId, string emailAddress, string fullName) : base(userId.Id, emailAddress, fullName)
        {
        }

        protected Manager()
        {
        }

        public UserId UserId
        {
            get { return new UserId(ActorGuid); }
        }
    }
}