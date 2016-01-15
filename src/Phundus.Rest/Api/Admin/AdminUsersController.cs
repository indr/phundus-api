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
                    UserGuid = s.UserGuid,
                    IsApproved = s.IsApproved,
                    IsLocked = s.IsLockedOut,
                    IsAdmin = s.RoleId == 2,
                    SignedUpAtUtc = s.SignedUpAtUtc,
                    LastLogInAtUtc = s.LastLogInAtUtc
                }).ToList()
            };
        }

        [PATCH("{userGuid}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid userGuid, AdminUsersPatchRequestContent requestContent)
        {
            if (requestContent.IsLocked.HasValue)
            {
                if (requestContent.IsLocked.Value)
                {
                    Dispatch(new LockUser(CurrentUserGuid, new UserGuid(requestContent.UserGuid)));
                }
                else
                {
                    Dispatch(new UnlockUser(CurrentUserGuid, new UserGuid(requestContent.UserGuid)));
                }
            }
            if (requestContent.IsAdmin.HasValue)
            {
                Dispatch(new ChangeUserRole(CurrentUserGuid, new UserGuid(requestContent.UserGuid),
                    requestContent.IsAdmin.Value ? UserRole.Admin : UserRole.User));
            }
            if (requestContent.IsApproved.HasValue && requestContent.IsApproved.Value)
            {
                Dispatch(new ApproveUser(CurrentUserGuid, new UserGuid(requestContent.UserGuid)));
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