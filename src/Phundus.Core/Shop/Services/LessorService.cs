namespace Phundus.Core.Shop.Services
{
    using System;
    using Common;
    using IdentityAndAccess.Queries;
    using Infrastructure;
    using Orders.Model;

    public interface ILessorService
    {
        /// <summary>
        /// </summary>
        /// <param name="lessorId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Lessor GetById(Guid lessorId);
    }

    public class LessorService : ILessorService
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IUserQueries _userQueries;

        public LessorService(IOrganizationQueries organizationQueries, IUserQueries userQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");

            _organizationQueries = organizationQueries;
            _userQueries = userQueries;
        }

        public Lessor GetById(Guid lessorId)
        {
            var organization = _organizationQueries.FindById(lessorId);
            if (organization != null)
                return ToLessor(organization);

            var user = _userQueries.FindById(lessorId);
            if (user != null)
                return ToLessor(user);

            throw new NotFoundException(String.Format("Lessor with id {0} not found.", lessorId));
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