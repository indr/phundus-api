namespace Phundus.Shop.Model
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;

    public interface ILessorService
    {
        /// <summary>
        /// </summary>
        /// <param name="lessorId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessor GetById(LessorId lessorId);

        ICollection<Integration.IdentityAccess.Manager> GetManagersForEmailNotification(LessorId lessorId);
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

        public ICollection<Integration.IdentityAccess.Manager> GetManagersForEmailNotification(LessorId lessorId)
        {
            if (lessorId == null) throw new ArgumentNullException("lessorId");

            var members = _membersWithRole.Manager(lessorId.Id, true);
            var user = _usersQueries.FindById(lessorId.Id);

            return ToManagers(members, user);
        }

        private static ICollection<Integration.IdentityAccess.Manager> ToManagers(IList<Integration.IdentityAccess.Manager> members, IUser user)
        {
            AssertionConcern.AssertArgumentNotNull(members, "Members must be provided.");

            var result = members;

            if (user != null)
                result.Add(new Integration.IdentityAccess.Manager(new UserId(user.UserId), user.EmailAddress, user.FullName));
            return result;
        }

        private static Lessor ToLessor(IUser user)
        {
            var lessorId = new LessorId(user.UserId);            
            return new Lessor(lessorId, user.FullName, user.PostalAddress, user.MobilePhone, user.EmailAddress, null, true);
        }

        private static Lessor ToLessor(OrganizationData organization)
        {
            var lessorId = new LessorId(organization.OrganizationId);
            return new Lessor(lessorId, organization.Name, organization.PostalAddress, organization.PhoneNumber, organization.EmailAddress, organization.Website, organization.PublicRental);
        }
    }
}