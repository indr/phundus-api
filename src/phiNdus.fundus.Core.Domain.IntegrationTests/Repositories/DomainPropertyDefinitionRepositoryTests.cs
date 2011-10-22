﻿using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class DomainPropertyDefinitionRepositoryTests : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IFieldDefinitionRepository>();
        }

        protected IFieldDefinitionRepository Sut { get; set; }

        [Test]
        public void Can_find_all()
        {
            using (var uow = UnitOfWork.Start())
            {
                var actual = Sut.FindAll();

                Assert.That(actual, Has.Count.EqualTo(11));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Name").And.Property("DataType").EqualTo(DataType.Text));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Preis").And.Property("DataType").EqualTo(DataType.Decimal));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Verfügbar").And.Property("DataType").EqualTo(DataType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Menge").And.Property("DataType").EqualTo(DataType.Integer));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Erfassungsdatum").And.Property("DataType").EqualTo(DataType.DateTime));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Reservierbar").And.Property("DataType").EqualTo(DataType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Ausleihbar").And.Property("DataType").EqualTo(DataType.Boolean));
            }
        }
    }
}