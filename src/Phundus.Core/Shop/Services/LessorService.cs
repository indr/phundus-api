namespace Phundus.Core.Shop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Queries;
    using Integration.IdentityAccess;
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
        private readonly IMembersWithRole _membersWithRole;
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUserQueries _userQueries;

        public LessorService(IOrganizationQueries organizationQueries, IUserQueries userQueries,
            IMembersWithRole membersWithRole)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            if (userQueries == null) throw new ArgumentNullException("userQueries");
            if (membersWithRole == null) throw new ArgumentNullException("membersWithRole");

            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
            _membersWithRole = membersWithRole;
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
            var members = _membersWithRole.Manager(lessorId);
            var user = _userQueries.FindById(lessorId);

            return ToManagers(members, user);
        }

        private static ICollection<Manager> ToManagers(IList<Manager> members, UserDto user)
        {
            AssertionConcern.AssertArgumentNotNull(members, "Members must be provided.");

            var result = members;

            if (user != null)
                result.Add(new Manager(user.Guid, user.FullName, user.Email));
            return result;
        }

        private static Lessor ToLessor(UserDto user)
        {
            return new Lessor(new LessorId(user.Guid), user.FirstName + " " + user.LastName);
        }

        private static Lessor ToLessor(OrganizationDetailDto organization)
        {
            return new Lessor(new LessorId(organization.Guid), organization.Name);
        }
    }
}