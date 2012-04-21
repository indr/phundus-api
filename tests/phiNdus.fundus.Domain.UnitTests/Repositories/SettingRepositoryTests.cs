using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class SettingRepositoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new SettingRepository();
        }

        #endregion

        private ISettingRepository Sut { get; set; }

        [Test]
        public void FindByKeyspace_with_trailing_dot_throws()
        {
            Assert.Throws<ArgumentException>(() => Sut.FindByKeyspace("space.with.trailing.dot."));
        }
    }
}