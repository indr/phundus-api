using NUnit.Framework;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.TestHelpers.Builders;

namespace phiNdus.fundus.Business.UnitTests.Security
{
    [TestFixture]
    public class SecurityContextTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new SecurityContext();
        }

        #endregion

        protected SecurityContext Sut { get; set; }

        [Test]
        public void Set_and_get_Session()
        {
            Assert.That(Sut.SecuritySession, Is.Null);
            Sut.SecuritySession = new SecuritySessionBuilder(null, "").Build();
            Assert.That(Sut.SecuritySession, Is.Not.Null);
        }
    }
}