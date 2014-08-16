namespace Phundus.Core
{
    using System.Security.Principal;

    public class AppServiceBase
    {
        public IIdentity Identity { get; set; }
    }
}