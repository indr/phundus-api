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
    using IdentityAccess.Application;
    using Integration.IdentityAccess;
    using Newtonsoft.Json;

    [RoutePrefix("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ApiControllerBase
    {
        private readonly IUsersQueries _usersQueries;

        public AdminUsersController(IUsersQueries usersQueries)
        {
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");
            _usersQueries = usersQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<AdminUser> Get()
        {
            var results = _usersQueries.Query();
            return new QueryOkResponseContent<AdminUser>
            {
                Results = results.Select(s => new AdminUser
                {
                    EmailAddress = s.EmailAddress,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    UserId = s.UserId,
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
                    Dispatch(new LockUser(CurrentUserId, new UserId(requestContent.UserId)));
                }
                else
                {
                    Dispatch(new UnlockUser(CurrentUserId, new UserId(requestContent.UserId)));
                }
            }
            if (requestContent.IsAdmin.HasValue)
            {
                Dispatch(new ChangeUserRole(CurrentUserId, new UserId(requestContent.UserId),
                    requestContent.IsAdmin.Value ? UserRole.Admin : UserRole.User));
            }
            if (requestContent.IsApproved.HasValue && requestContent.IsApproved.Value)
            {
                Dispatch(new ApproveUser(CurrentUserId, new UserId(requestContent.UserId)));
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