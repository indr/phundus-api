﻿namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using System.Web.Http;
    using System.Web.Security;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Auth;
    using Castle.Transactions;
    using Common.Resources;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("/api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        private readonly IMembershipQueryService _membershipQueryService;
        private readonly IUserQueryService _userQueryService;

        public SessionsController(IUserQueryService userQueryService, IMembershipQueryService membershipQueryService)
        {
            if (userQueryService == null) throw new ArgumentNullException("userQueryService");
            if (membershipQueryService == null) throw new ArgumentNullException("membershipQueryService");

            _userQueryService = userQueryService;
            _membershipQueryService = membershipQueryService;
        }

        [POST("")]
        [Transaction]
        [AllowAnonymous]
        public virtual SessionsPostOkResponseContent Post(SessionsPostRequestContent requestContent)
        {
            CheckForMaintenanceMode(requestContent.Username);

            if (!Membership.ValidateUser(requestContent.Username, requestContent.Password))
                throw new AuthenticationException("Unbekannter Fehler bei der Anmeldung.");

            FormsAuthentication.SetAuthCookie(requestContent.Username, false);

            var user = _userQueryService.FindByUsername(requestContent.Username);

            var memberships = _membershipQueryService.FindByUserId(user.UserId)
                .Select(each => new Memberships
                {
                    IsManager = each.MembershipRole == "Manager",
                    OrganizationId = each.OrganizationGuid,
                    OrganizationName = each.OrganizationName,
                    OrganizationUrl = each.OrganizationUrl
                }).ToList();


            var auth = new Auth();
            UserRole role = null;
            if (!auth.Roles.TryGetValue(user.RoleName.ToLowerInvariant(), out role))
                role = auth.Roles["user"];

            return new SessionsPostOkResponseContent
            {
                Memberships = memberships,
                Role = new Role
                {
                    BitMask = role.BitMask,
                    Title = role.Title
                },
                UserId = user.UserId,
                Username = user.EmailAddress,
                FullName = user.FullName
            };
        }

        [DELETE("")]
        [Transaction]
        [AllowAnonymous]
        public virtual void Delete()
        {
            FormsAuthentication.SignOut();
        }
    }

    public class SessionsPostRequestContent
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class SessionsPostOkResponseContent
    {
        [JsonProperty("memberships")]
        public List<Memberships> Memberships { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }

    public class Role
    {
        [JsonProperty("bitMask")]
        public int BitMask { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}