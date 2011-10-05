using System;
using Castle.Windsor;
using NHibernate.Linq;
using NHibernate.Util;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Order = NHibernate.Criterion.Order;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    public class BaseTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }


        protected User CreatePersistentUser()
        {
            return CreatePersistentUser("user@example.com");
        }

        protected User CreatePersistentUser(string email)
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