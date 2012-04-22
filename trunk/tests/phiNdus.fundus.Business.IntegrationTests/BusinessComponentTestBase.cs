using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;

namespace phiNdus.fundus.Business.IntegrationTests
{
    public class BusinessComponentTestBase<TSut> : ComponentTestBase<TSut>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            IoC.Container.Install(new phiNdus.fundus.Business.Installer());
        }
    }
}
