using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    public class TestBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            GlobalContainer.Initialize(new WindsorContainer());
        }

        [TearDown]
        public virtual void TearDown()
        {
            GlobalContainer.Container.Dispose();
        }
    }
}