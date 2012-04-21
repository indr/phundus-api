using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests
{
    public abstract class SettingsTestFixture<TSut> : DomainComponentTestBase<TSut>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Delete("from Setting");
                uow.TransactionalFlush();
            }

            Sut = CreateSut();
        }

        

        protected abstract TSut CreateSut();

        protected void InsertSetting(string key, string value)
        {
            using (var uow = UnitOfWork.Start())
            {
                var setting = new Setting(key);
                setting.StringValue = value;
                UnitOfWork.CurrentSession.Save(setting);
                uow.TransactionalFlush();
            }
        }
    }
}
