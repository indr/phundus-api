namespace Phundus.Common
{
    using System;
    using Domain.Model;

    public class ProviderUserKey
    {
        public ProviderUserKey(object providerUserKey)
        {
            if (providerUserKey == null) throw new ArgumentNullException("providerUserKey");

            var parts = Convert.ToString(providerUserKey).Split('/');
            UserId = new CurrentUserId(Convert.ToInt32(parts[0]));
            UserGuid = new CurrentUserGuid(new Guid(parts[1]));
        }

        public ProviderUserKey(int userId, Guid userGuid)
        {
            UserId = new CurrentUserId(userId);
            UserGuid = new CurrentUserGuid(userGuid);
        }

        public CurrentUserId UserId { get; set; }

        public CurrentUserGuid UserGuid { get; set; }

        public override string ToString()
        {
            return String.Format("{0}/{1}", UserId.Id, UserGuid.Id.ToString("D"));
        }
    }
}