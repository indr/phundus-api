using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
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
            Sut.SecuritySession = SessionHelper.CreateSession(null, "");
            Assert.That(Sut.SecuritySession, Is.Not.Null);
        }
    }
}