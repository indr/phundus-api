namespace Phundus.Core.Shop.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Queries;
    using Orders.Model;

    public interface ILessorService
    {
        /// <summary>
        /// </summary>
        /// <param name="lessorId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessor GetById(Guid lessorId);

        /// <summary>
        /// </summary>
        /// <param name="lessorId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessor GetById(LessorId lessorId);

        ICollection<Manager> GetManagers(Guid lessorId);
    }

    public class LessorService : ILessorService
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUserQueries _userQueries;
        private readonly IMemberInRoleQueries _memberInRoleQueries;

        public LessorService(IOrganizationQueries organizationQueries, IUserQueries userQueries, IMemberInRoleQueries memberInRoleQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(memberInRoleQueries, "MemberInRoleQueries must be provided.");

            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
            _memberInRoleQueries = memberInRoleQueries;
        }

        public Lessor GetById(Guid lessorId)
        {
            var organization = _organizationQueries.FindById(lessorId);
            if (organization != null)
                return ToLessor(organization);

            var user = _userQueries.FindById(lessorId);
            if (user != null)
                return ToLessor(user);

            throw new NotFoundException(String.Format("Lessor {0} not found.", lessorId));
        }

        public Lessor GetById(LessorId lessorId)
        {
            return GetById(lessorId.Id);
        }

        public ICollection<Manager> GetManagers(Guid lessorId)
        {
            var members = _memberInRoleQueries.Chiefs(lessorId);
            var user = _userQueries.FindById(lessorId);

            return ToManagers(members, user);
        }

        private static ICollection<Manager> ToManagers(IEnumerable<MemberDto> members, UserDto user)
        {
            AssertionConcern.AssertArgumentNotNull(members, "Members must be provided.");

            var result = members.Select(each => new Manager(each.Guid, each.FullName, each.EmailAddress)).ToList();

            if (user != null)
                result.Add(new Manager(user.Guid, user.FullName, user.Email));
            return result;
        }

        private static Lessor ToLessor(UserDto user)
        {
            return new Lessor(user.Guid, user.FirstName + " " + user.LastName);
        }

        private static Lessor ToLessor(OrganizationDetailDto organization)
        {
            return new Lessor(organization.Guid, organization.Name);
        }
    }
}