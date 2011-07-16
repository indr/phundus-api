using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.AcceptanceTests.AppDriver
{
    public class AdminApi
    {
        public void DeleteUser(string email)
        {
            var userService = new UserService();
            var user = userService.GetUser(email);
            if (user != null)
                userService.DeleteUser(email);
        }

        public void CreateUser(string email)
        {
            var userService = new UserService();
            if (userService.GetUser("dave@example.com") == null)
                userService.CreateUser("dave@example.com", "password", "", "");
        }
    }
}