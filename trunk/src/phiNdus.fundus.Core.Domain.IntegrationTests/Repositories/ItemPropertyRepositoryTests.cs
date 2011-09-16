using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class ItemPropertyRepositoryTests : BaseTestFixture
    {
        [Test]
        public void Can_create()
        {
            new ItemPropertyRepository();
        }

        [Test]
        public void Can_find_all()
        {
            var sut = new ItemPropertyRepository();
            using (var uow = UnitOfWork.Start())
            {
                var actual = sut.FindAll();
                Assert.That(actual.Count, Is.EqualTo(1));
            }
        }
    }
}
