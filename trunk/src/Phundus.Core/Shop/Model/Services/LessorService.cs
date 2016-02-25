﻿namespace Phundus.Shop.Services
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Model;

    public interface ILessorService
    {
        /// <summary>
        /// </summary>
        /// <param name="lessorId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessor GetById(LessorId lessorId);

        ICollection<Manager> GetManagersForEmailNotification(LessorId lessorId);
    }

    public class LessorService : ILessorService
    {
        private readonly IMembersWithRole _membersWithRole;
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUsersQueries _usersQueries;

        public LessorService(IOrganizationQueries organizationQueries, IUsersQueries usersQueries,
            IMembersWithRole membersWithRole)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            if (usersQueries == null) throw new ArgumentNullException("usersQueries");
            if (membersWithRole == null) throw new ArgumentNullException("membersWithRole");

            _organizationQueries = organizationQueries;
            _usersQueries = usersQueries;
            _membersWithRole = membersWithRole;
        }

        public Lessor GetById(LessorId lessorId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");

            var organization = _organizationQueries.FindById(lessorId.Id);
            if (organization != null)
                return ToLessor(organization);

            var user = _usersQueries.FindById(lessorId.Id);
            if (user != null)
                return ToLessor(user);

            throw new NotFoundException(String.Format("Lessor {0} not found.", lessorId));
        }

        public ICollection<Manager> GetManagersForEmailNotification(LessorId lessorId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");

            var members = _membersWithRole.Manager(lessorId.Id, true);
            var user = _usersQueries.FindById(lessorId.Id);

            return ToManagers(members, user);
        }

        private static ICollection<Manager> ToManagers(IList<Manager> members, IUser user)
        {
            AssertionConcern.AssertArgumentNotNull(members, "Members must be provided.");

            var result = members;

            if (user != null)
                result.Add(new Manager(new UserId(user.UserId), user.EmailAddress, user.FullName));
            return result;
        }

        private static Lessor ToLessor(IUser user)
        {
            return new Lessor(new LessorId(user.UserId), user.FullName, true);
        }

        private static Lessor ToLessor(IOrganization organization)
        {
            return new Lessor(new LessorId(organization.OrganizationId), organization.Name, organization.PublicRental);
        }
    }
}