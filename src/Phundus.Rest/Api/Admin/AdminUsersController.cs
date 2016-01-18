namespace Phundus.Rest.Api.Admin
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Users.Commands;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ApiControllerBase
    {
        private readonly IUserQueries _userQueries;

        public AdminUsersController(IUserQueries userQueries)
        {
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            _userQueries = userQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<AdminUser> Get()
        {
            var results = _userQueries.Query();
            return new QueryOkResponseContent<AdminUser>
            {
                Results = results.Select(s => new AdminUser
                {
                    EmailAddress = s.EmailAddress,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    UserId = s.UserGuid,
                    IsApproved = s.IsApproved,
                    IsLocked = s.IsLockedOut,
                    IsAdmin = s.RoleId == 2,
                    SignedUpAtUtc = s.SignedUpAtUtc,
                    LastLogInAtUtc = s.LastLogInAtUtc
                }).ToList()
            };
        }

        [PATCH("{userId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid userId, AdminUsersPatchRequestContent requestContent)
        {
            if (requestContent.IsLocked.HasValue)
            {
                if (requestContent.IsLocked.Value)
                {
                    Dispatch(new LockUser(CurrentUserId, new UserGuid(requestContent.UserId)));
                }
                else
                {
                    Dispatch(new UnlockUser(CurrentUserId, new UserGuid(requestContent.UserId)));
                }
            }
            if (requestContent.IsAdmin.HasValue)
            {
                Dispatch(new ChangeUserRole(CurrentUserId, new UserGuid(requestContent.UserId),
                    requestContent.IsAdmin.Value ? UserRole.Admin : UserRole.User));
            }
            if (requestContent.IsApproved.HasValue && requestContent.IsApproved.Value)
            {
                Dispatch(new ApproveUser(CurrentUserId, new UserGuid(requestContent.UserId)));
            }
            return NoContent();
        }
    }

    public class AdminUsersPatchRequestContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("isApproved")]
        public bool? IsApproved { get; set; }

        [JsonProperty("isLocked")]
        public bool? IsLocked { get; set; }

        [JsonProperty("isAdmin")]
        public bool? IsAdmin { get; set; }
    }
}