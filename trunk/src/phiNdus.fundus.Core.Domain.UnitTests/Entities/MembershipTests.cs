using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class MembershipTests
    {
        [Test]
        public void Set_and_get_SessionKey()
        {
            var sut = new Membership();
            Assert.That(sut.SessionKey, Is.Null);
            sut.SessionKey = "1234";
            Assert.That(sut.SessionKey, Is.EqualTo("1234"));
        }
    }
}