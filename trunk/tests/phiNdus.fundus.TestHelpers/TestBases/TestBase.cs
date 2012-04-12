using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class TestBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
        }

        [TearDown]
        public virtual void TearDown()
        {
            IoC.Container.Dispose();
        }
    }
}