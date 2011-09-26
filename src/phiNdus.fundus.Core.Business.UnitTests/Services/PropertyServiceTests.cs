using System.Collections.Generic;
using NUnit.Framework;
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
        }

        #endregion

        protected PropertyService Sut { get; set; }

        protected IUnitOfWork FakeUnitOfWork { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IDomainPropertyDefinitionRepository>() == null)
                FakePropertyDefRepo = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();
        }

        protected IDomainPropertyDefinitionRepository FakePropertyDefRepo { get; set; }

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
    }
}