using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    [TestFixture]
    public class UserRepositoryTests : RepositoryTestFixture
    {
        

        [Test]
        public void Can_get_user()
        {
            using (UnitOfWork.Start())
            {
                var repo = new UserRepository();
                var user = repo.Get(1);
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(1));
                Assert.That(user.FirstName, Is.EqualTo("Ted"));
                Assert.That(user.LastName, Is.EqualTo("Mosby"));
            }
        }

    }
}
