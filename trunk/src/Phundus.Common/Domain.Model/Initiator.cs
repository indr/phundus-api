namespace Phundus.Common.Domain.Model
{
    public class Initiator : Actor
    {
        public Initiator(InitiatorId initiatorId, string emailAddress, string fullName)
            : base(initiatorId.Id, emailAddress, fullName)
        {
        }

        protected Initiator()
        {
        }

        public InitiatorId InitiatorId
        {
            get { return new InitiatorId(ActorGuid); }
        }
    }
}