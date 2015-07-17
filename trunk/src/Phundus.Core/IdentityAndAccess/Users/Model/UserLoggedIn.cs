namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserLoggedIn : DomainEvent
    {
        protected UserLoggedIn()
        {
        }

        public UserLoggedIn(int userId)
        {
            UserId = userId;
        }

        [DataMember(Order = 1)]
        public int UserId { get; set; }
    }
}