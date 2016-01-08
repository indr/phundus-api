namespace Phundus.Rest.Api.Admin
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ApiControllerBase
    {
        private readonly IUserQueries _userQueries;

        public AdminUsersController(IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            _userQueries = userQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<AdminUser> Get()
        {
            var results = _userQueries.All();
            return new QueryOkResponseContent<AdminUser>
            {
                Results = results.Select(s => new AdminUser
                {
                    EmailAddress = s.Email,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    UserGuid = s.Guid,
                    UserId = s.Id,
                    IsApproved = s.IsApproved,
                    IsLocked = s.IsLockedOut,
                    IsAdmin = s.RoleName == "Admin"
                }).ToList()
            };
        }

        [PATCH("{userId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int userId, AdminUsersPatchRequestContent requestContent)
        {
            if (requestContent.IsLocked.HasValue)
            {
                if (requestContent.IsLocked.Value)
                {
                    Dispatch(new LockUser(CurrentUserId, new UserGuid(requestContent.UserGuid)));
                }
                else
                {
                    Dispatch(new UnlockUser(CurrentUserId, new UserGuid(requestContent.UserGuid)));
                }
            }
            if (requestContent.IsAdmin.HasValue)
            {
                Dispatch(new ChangeUserRole(CurrentUserId, new UserGuid(requestContent.UserGuid),
                    requestContent.IsAdmin.Value ? UserRole.Admin : UserRole.User));
            }
            if (requestContent.IsApproved.HasValue && requestContent.IsApproved.Value)
            {
                Dispatch(new ApproveUser(CurrentUserId, new UserGuid(requestContent.UserGuid)));
            }
            return NoContent();
        }
    }

    public class AdminUsersPatchRequestContent
    {
        [JsonProperty("userGuid")]
        public Guid UserGuid { get; set; }

        [JsonProperty("isApproved")]
        public bool? IsApproved { get; set; }

        [JsonProperty("isLocked")]
        public bool? IsLocked { get; set; }

        [JsonProperty("isAdmin")]
        public bool? IsAdmin { get; set; }
    }
}