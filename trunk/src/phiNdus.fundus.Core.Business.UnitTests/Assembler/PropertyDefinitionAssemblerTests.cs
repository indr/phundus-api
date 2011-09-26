using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class PropertyDefinitionAssemblerTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            PropertyDefinition = new DomainPropertyDefinition(1, 2, "Caption", DomainPropertyType.Text);

            PropertyDto = new PropertyDto();
            PropertyDto.Id = 1;
            PropertyDto.Version = 2;
            PropertyDto.Caption = "Caption";
            PropertyDto.DataType = PropertyDataType.Text;
        }

        #endregion

        protected PropertyDto PropertyDto { get; set; }
        protected DomainPropertyDefinition PropertyDefinition { get; set; }

        protected IDomainPropertyDefinitionRepository FakePropertyDefRepo { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IDomainPropertyDefinitionRepository>() == null)
            {
                FakePropertyDefRepo = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();
                FakePropertyDefRepo.Expect(x => x.Get(1)).Return(PropertyDefinition);
            }
        }

        [Test]
        public void CreateDto_regarding_DataType()
        {
            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Boolean)).DataType,
                Is.EqualTo(PropertyDataType.Boolean));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.DateTime)).DataType,
                Is.EqualTo(PropertyDataType.DateTime));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Decimal)).DataType,
                Is.EqualTo(PropertyDataType.Decimal));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Integer)).DataType,
                Is.EqualTo(PropertyDataType.Integer));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Text)).DataType,
                Is.EqualTo(PropertyDataType.Text));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = PropertyDefinitionAssembler.CreateDto(PropertyDefinition);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.Caption, Is.EqualTo("Caption"));
            Assert.That(dto.DataType, Is.EqualTo(PropertyDataType.Text));
        }

        [Test]
        public void CreateDto_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PropertyDefinitionAssembler.CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDtos_returns_array()
        {
            var propertyDefinitions = new List<DomainPropertyDefinition>();
            propertyDefinitions.Add(new DomainPropertyDefinition(1, "Name 1", DomainPropertyType.Boolean));
            propertyDefinitions.Add(new DomainPropertyDefinition(2, "Name 2", DomainPropertyType.Text));
            var dtos = PropertyDefinitionAssembler.CreateDtos(propertyDefinitions);

            Assert.That(dtos, Has.Length.EqualTo(2));
        }

        [Test]
        public void CreateDtos_with_subjects_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PropertyDefinitionAssembler.CreateDtos(null));
            Assert.That(ex.ParamName, Is.EqualTo("subjects"));
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = PropertyDefinitionAssembler.CreateDomainObject(
                    new PropertyDto
                        {
                            Id = 1,
                            Version = 2,
                            Caption = "Caption",
                            DataType = PropertyDataType.Text
                        }
                );

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.Version, Is.EqualTo(0));
            Assert.That(domainObject.Name, Is.EqualTo("Caption"));
            Assert.That(domainObject.DataType, Is.EqualTo(DomainPropertyType.Text));
        }

        [Test]
        public void CreateDomainObject_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PropertyDefinitionAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            PropertyDto.Caption = "Caption (Updated)";

            var domainObject = PropertyDefinitionAssembler.UpdateDomainObject(PropertyDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(1));
            Assert.That(domainObject.Version, Is.EqualTo(2));
            Assert.That(domainObject.Name, Is.EqualTo("Caption (Updated)"));
            Assert.That(domainObject.DataType, Is.EqualTo(DomainPropertyType.Text));
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            FakePropertyDefRepo = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Get(1)).Return(null);

            Assert.Throws<EntityNotFoundException>(() => PropertyDefinitionAssembler.UpdateDomainObject(PropertyDto));
        }

        [Test]
        public void UpdateDomainObject_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PropertyDefinitionAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));   
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();

            PropertyDto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => PropertyDefinitionAssembler.UpdateDomainObject(PropertyDto));
        }
    }
}