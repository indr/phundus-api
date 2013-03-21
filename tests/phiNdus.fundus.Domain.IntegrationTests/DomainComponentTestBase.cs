using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Order = NHibernate.Criterion.Order;

namespace phiNdus.fundus.Domain.IntegrationTests
{
    public class DomainComponentTestBase<TSut> : ComponentTestBase<TSut>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            IoC.Container.Install(new Installer());
        }

        protected Article CreatePersistentArticle()
        {
            using (var uow = UnitOfWork.Start())
            {
                var result = new Article();
                var organization = new Organization();
                organization.Name = Guid.NewGuid().ToString("N");
                UnitOfWork.CurrentSession.Save(organization);
                result.Organization = organization;
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