using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.AcceptanceTests.AppDriver
{
    public class AdminApi
    {
        public void DeleteUser(string email)
        {
            var userService = new UserService();
            UserDto user = userService.GetUser(email);
            if (user != null)
                userService.DeleteUser(email);
        }
    }
}