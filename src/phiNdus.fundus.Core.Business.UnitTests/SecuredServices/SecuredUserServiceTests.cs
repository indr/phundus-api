using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.SecuredServices;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices
{
    [TestFixture]
    internal class SecuredUserServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new SecuredUserService();
        }

        #endregion

        protected SecuredUserService Sut { get; set; }

        #region GetUser
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUser_with_sessionKey_null_throws()
        {
            Sut.GetUser(null, "");
        }
        #endregion
    }
}