using Castle.Windsor;
using NUnit.Framework;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

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