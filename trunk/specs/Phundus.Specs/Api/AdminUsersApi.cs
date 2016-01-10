namespace Phundus.Specs.Api
{
    public class AdminUsersApi : ApiBase
    {
        public AdminUsersApi()
            : base("admin/users/{userGuid}")
        {
        }
    }
}