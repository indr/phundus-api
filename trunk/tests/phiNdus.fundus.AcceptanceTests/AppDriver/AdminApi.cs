using phiNdus.fundus.Core.Business.Dto;
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
            if (userService.GetUser(email) == null)
                userService.CreateUser(email, "password", "", "");
        }

        public UserDto GetUser(string email)
        {
            var userService = new UserService();
            return userService.GetUser(email);
        }
    }
}