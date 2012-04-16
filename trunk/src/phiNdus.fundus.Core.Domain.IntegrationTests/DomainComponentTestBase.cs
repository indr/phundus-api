using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Order = NHibernate.Criterion.Order;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    public class DomainComponentTestBase<TSut> : ComponentTestBase<TSut>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            IoC.Container.Install(new Installer());
        }

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

        protected Article CreatePersistentArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                var result = new Article();
                UnitOfWork.CurrentSession.Save(result);
                return result;
            }
        }

        protected OrderItem CreateTransientOrderItem()
        {
            var random = new Random();
            return new OrderItem
                       {
                           From = DateTime.Today.AddDays(random.Next(0, 3)),
                           To = DateTime.Today.AddDays(random.Next(4, 7))
                       };
        }
    }
}