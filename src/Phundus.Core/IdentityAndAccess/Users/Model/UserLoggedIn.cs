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

        public UserLoggedIn(int userId, string emailAddress)
        {
            UserId = userId;
            EmailAddress = emailAddress;
        }

        [DataMember(Order = 1)]
        public int UserId { get; set; }

        [DataMember(Order = 2)]
        public string EmailAddress { get; set; }
    }
}