using System;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;
using Order = NHibernate.Criterion.Order;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class ComponentTestBase<TSut> : TestBase
    {
        public ComponentTestBase() : base()
        {

        }

        protected TSut Sut { get; set; }


        protected User CreateAndPersistUser()
        {
            return CreateAndPersistUser("user@example.com");
        }

        protected User CreateAndPersistUser(string email)
        {
            var users = IoC.Resolve<IUserRepository>();
            var roles = IoC.Resolve<IRoleRepository>();

            var result = users.FindByEmail(email);
            if (result == null)
            {
                result = new User();
                result.Membership.Email = email;
                result.Role = roles.FindFirst(new Order[0]);
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
