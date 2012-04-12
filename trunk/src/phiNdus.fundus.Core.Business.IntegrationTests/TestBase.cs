using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.IntegrationTests
{
    public class TestBase<TSut> : ComponentTestBase<TSut>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            IoC.Container.Install(new phiNdus.fundus.Core.Business.Installer());
        }
    }
}
