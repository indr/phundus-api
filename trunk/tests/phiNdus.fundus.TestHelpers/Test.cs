using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.TestHelpers
{
    [TestFixture]
    public class TestFixtureTest : ComponentTestBase<object>
    {

        [Test]
        public void Test()
        {
            Assert.That(true, Is.True);
        }
    }
}
