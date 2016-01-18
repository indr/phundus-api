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
            
            UserId = new CurrentUserId(new Guid(parts.Length == 2 ? parts[1] : parts[0]));
        }

        public ProviderUserKey(Guid userGuid)
        {
            UserId = new CurrentUserId(userGuid);
        }

        public CurrentUserId UserId { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", UserId.Id.ToString("D"));
        }
    }
}