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
            Sut = IoC.Resolve<IDomainPropertyDefinitionRepository>();
        }

        protected IDomainPropertyDefinitionRepository Sut { get; set; }

        [Test]
        public void Can_find_all()
        {
            using (var uow = UnitOfWork.Start())
            {
                var actual = Sut.FindAll();

                Assert.That(actual, Has.Count.EqualTo(11));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Name").And.Property("DataType").EqualTo(FieldType.Text));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Preis").And.Property("DataType").EqualTo(FieldType.Decimal));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Verfügbar").And.Property("DataType").EqualTo(FieldType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Menge").And.Property("DataType").EqualTo(FieldType.Integer));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Erfassungsdatum").And.Property("DataType").EqualTo(FieldType.DateTime));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Reservierbar").And.Property("DataType").EqualTo(FieldType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Ausleihbar").And.Property("DataType").EqualTo(FieldType.Boolean));
            }
        }
    }
}