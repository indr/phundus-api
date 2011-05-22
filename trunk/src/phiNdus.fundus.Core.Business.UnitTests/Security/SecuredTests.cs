using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    internal class SecuredTests : BaseTestFixture
    {
        [Test]
        public void With_session_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Secured.With(null));
        }
    }
}