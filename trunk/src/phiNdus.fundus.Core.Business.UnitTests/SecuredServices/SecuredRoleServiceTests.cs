using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices {
    [TestFixture]
    public class SecuredRoleServiceTests : BaseTestFixture {

        [SetUp]
        public void SetUpSut() {
            this.Sut = new SecuredRoleService();
        }

        protected SecuredRoleService Sut { get; set; }

        [Test]
        public void Get_roles_returns_existing_roles() {
            var roles = this.Sut.GetRoles(null);

            Assert.That(roles.Count(), Is.EqualTo(2));
            Assert.That(roles.SingleOrDefault(r => r.Name == "User"), Is.Not.Null);
            Assert.That(roles.SingleOrDefault(r => r.Name == "Admin"), Is.Not.Null);
        }
    }
}
