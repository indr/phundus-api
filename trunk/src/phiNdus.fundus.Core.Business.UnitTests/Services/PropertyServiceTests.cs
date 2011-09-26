using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    public class PropertyServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new PropertyService();

            DomainPropertyDefinition = new DomainPropertyDefinition(1, 2, "Caption", DomainPropertyType.Text);
        }

        #endregion

        protected PropertyService Sut { get; set; }

        protected IUnitOfWork FakeUnitOfWork { get; set; }
        protected IDomainPropertyDefinitionRepository FakePropertyDefRepo { get; set; }

        protected DomainPropertyDefinition DomainPropertyDefinition { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IDomainPropertyDefinitionRepository>() == null)
            {
                FakePropertyDefRepo = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();
                FakePropertyDefRepo.Expect(x => x.Get(DomainPropertyDefinition.Id)).Return(DomainPropertyDefinition);
                FakePropertyDefRepo.Expect(x => x.Save(Arg<DomainPropertyDefinition>.Is.Anything)).Return(DomainPropertyDefinition);
            }
        }

        [Test]
        public void CreateProperty_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();

            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.CreateProperty(new PropertyDto());

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void CreateProperty_stores_property_in_repository()
        {
            FakePropertyDefRepo = GenerateAndRegisterMock<IDomainPropertyDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Save(Arg<DomainPropertyDefinition>.Is.NotNull));
            Sut.CreateProperty(new PropertyDto());

            FakePropertyDefRepo.VerifyAllExpectations();
        }

        [Test]
        public void CreateProperty_returns_id()
        {
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Save(Arg<DomainPropertyDefinition>.Is.Anything)).Return(new DomainPropertyDefinition(1, "Caption", DomainPropertyType.Text));
            var actual = Sut.CreateProperty(new PropertyDto());

            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void CreateProperty_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.CreateProperty(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void GetProperty_calls_repository_Get()
        {
            FakePropertyDefRepo = GenerateAndRegisterMock<IDomainPropertyDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Get(1)).Return(null);
            Sut.GetProperty(1);

            FakePropertyDefRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetProperty_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            PropertyDto actual = Sut.GetProperty(1);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.EqualTo(1));
            Assert.That(actual.Version, Is.EqualTo(2));
        }

        [Test]
        public void GetProperty_with_invalid_id_returns_null()
        {
            GenerateAndRegisterMissingStubs();

            Assert.That(Sut.GetProperty(101), Is.Null);
        }

        [Test]
        public void GetProperties_calls_repository_FindAll()
        {
            FakePropertyDefRepo = GenerateAndRegisterMock<IDomainPropertyDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.FindAll()).Return(new List<DomainPropertyDefinition>());
            Sut.GetProperties();

            FakePropertyDefRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetProperties_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var propertyDefinitions = new List<DomainPropertyDefinition>();
            propertyDefinitions.Add(new DomainPropertyDefinition(1, "Name 1", DomainPropertyType.Boolean));
            propertyDefinitions.Add(new DomainPropertyDefinition(1, "Name 2", DomainPropertyType.Text));
            FakePropertyDefRepo.Expect(x => x.FindAll()).Return(propertyDefinitions);

            var dtos = Sut.GetProperties();

            Assert.That(dtos, Has.Length.EqualTo(2));
            Assert.That(dtos, Has.Some.Property("Caption").EqualTo("Name 1"));
            Assert.That(dtos, Has.Some.Property("Caption").EqualTo("Name 2"));
        }

        [Test]
        public void UpdateProperty_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();

            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.UpdateProperty(new PropertyDto { Id = 1, Version = 2 });

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void UpdateProperty_stores_property_in_repository()
        {
            FakePropertyDefRepo = GenerateAndRegisterMock<IDomainPropertyDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Stub(x => x.Get(1)).Return(DomainPropertyDefinition);
            FakePropertyDefRepo.Expect(x => x.Save(Arg<DomainPropertyDefinition>.Is.Equal(DomainPropertyDefinition))).Return(DomainPropertyDefinition);
            Sut.UpdateProperty(new PropertyDto { Id = 1, Version = 2 });

            FakePropertyDefRepo.VerifyAllExpectations();
        }

        [Test]
        public void UpdateProperty_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateProperty(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }
    }
}