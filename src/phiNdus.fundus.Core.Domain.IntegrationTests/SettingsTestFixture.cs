﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    public abstract class SettingsTestFixture<TSut> : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Delete("from Setting");
                uow.TransactionalFlush();
            }

            Sut = CreateSut();
        }

        protected TSut Sut { get; private set; }

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
