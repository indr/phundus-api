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

        public LessorService(IOrganizationQueries organizationQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");

            _organizationQueries = organizationQueries;
        }

        public Lessor GetById(Guid lessorId)
        {
            var organization = _organizationQueries.FindById(lessorId);
            if (organization == null)
                throw new NotFoundException(String.Format("Lessor with id {0} not found", lessorId));

            return ToLessor(organization);
        }

        private static Lessor ToLessor(OrganizationDetailDto organization)
        {
            return new Lessor(organization.Guid, organization.Name);
        }
    }
}