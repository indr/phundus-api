using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    internal class SettingRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new SettingRepository();
        }

        private ISettingRepository Sut { get; set; }

        [Test]
        public void FindByKeyspace_with_trailing_dot_throws()
        {
            Assert.Throws<ArgumentException>(() => Sut.FindByKeyspace("space.with.trailing.dot."));
        }
    }
}