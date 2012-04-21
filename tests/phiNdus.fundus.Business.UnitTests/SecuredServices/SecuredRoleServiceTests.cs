using System.Linq;
using NUnit.Framework;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Business.UnitTests.SecuredServices
{
    [TestFixture]
    public class SecuredRoleServiceTests : UnitTestBase<SecuredRoleService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new SecuredRoleService();
        }

        #endregion

        [Test]
        public void Get_roles_returns_existing_roles()
        {
            var roles = Sut.GetRoles(null);

            Assert.That(roles.Count(), Is.EqualTo(2));
            Assert.That(roles.SingleOrDefault(r => r.Name == "User"), Is.Not.Null);
            Assert.That(roles.SingleOrDefault(r => r.Name == "Admin"), Is.Not.Null);
        }
    }
}