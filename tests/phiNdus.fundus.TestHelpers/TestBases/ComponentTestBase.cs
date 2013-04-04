using System;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Order = NHibernate.Criterion.Order;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ComponentTestBase<TSut> : TestBase
    {
        public ComponentTestBase() : base()
        {

        }

        protected TSut Sut { get; set; }


        protected Organization CreateAndPersistOrganization(string name = "Pfadi Lego")
        {
            var organizations = GlobalContainer.Resolve<IOrganizationRepository>();

            var result = organizations.FindByName(name);
            if (result == null)
            {
                result = new Organization();
                result.Name = name;
                organizations.Save(result);
            }
            return result;
        }

        protected User CreateAndPersistUser()
        {
            return CreateAndPersistUser("user@example.com");
        }

        protected User CreateAndPersistUser(string email)
        {
            var users = GlobalContainer.Resolve<IUserRepository>();
            var roles = GlobalContainer.Resolve<IRoleRepository>();

            var result = users.FindByEmail(email);
            if (result == null)
            {
                result = new User();
                result.Membership.Email = email;
                result.Role = roles.FindFirst(null);
                users.Save(result);
            }
            return result;
        }

        protected void Transactional(Action action)
        {
            using (var uow = UnitOfWork.Start())
            {
                action();
                uow.TransactionalFlush();
            }
        }
    }
}
