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
            
            UserGuid = new CurrentUserGuid(new Guid(parts.Length == 2 ? parts[1] : parts[0]));
        }

        public ProviderUserKey(Guid userGuid)
        {
            UserGuid = new CurrentUserGuid(userGuid);
        }

        public CurrentUserGuid UserGuid { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", UserGuid.Id.ToString("D"));
        }
    }
}