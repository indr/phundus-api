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

            Domain = new FieldDefinition(1, 2, "Caption", DataType.Text, true);

            Dto = new PropertyDto();
            Dto.Id = 1;
            Dto.Version = 2;
            Dto.Caption = "Caption";
            Dto.DataType = PropertyDataType.Text;
        }

        #endregion

        protected PropertyDto Dto { get; set; }
        protected FieldDefinition Domain { get; set; }

        protected IFieldDefinitionRepository FakePropertyDefRepo { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IFieldDefinitionRepository>() == null)
            {
                FakePropertyDefRepo = GenerateAndRegisterStub<IFieldDefinitionRepository>();
                FakePropertyDefRepo.Expect(x => x.Get(1)).Return(Domain);
            }
        }

        [Test]
        public void CreateDto_regarding_DataType()
        {
            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Boolean)).DataType,
                Is.EqualTo(PropertyDataType.Boolean));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.DateTime)).DataType,
                Is.EqualTo(PropertyDataType.DateTime));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Decimal)).DataType,
                Is.EqualTo(PropertyDataType.Decimal));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Integer)).DataType,
                Is.EqualTo(PropertyDataType.Integer));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Text)).DataType,
                Is.EqualTo(PropertyDataType.Text));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = PropertyDefinitionAssembler.CreateDto(Domain);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.Caption, Is.EqualTo("Caption"));
            Assert.That(dto.DataType, Is.EqualTo(PropertyDataType.Text));
            Assert.That(dto.IsSystemProperty, Is.True);
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
            var propertyDefinitions = new List<FieldDefinition>();
            propertyDefinitions.Add(new FieldDefinition(1, "Name 1", DataType.Boolean));
            propertyDefinitions.Add(new FieldDefinition(2, "Name 2", DataType.Text));
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
                            DataType = PropertyDataType.Text,
                            IsSystemProperty = true
                        }
                );

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.Version, Is.EqualTo(0));
            Assert.That(domainObject.Name, Is.EqualTo("Caption"));
            Assert.That(domainObject.DataType, Is.EqualTo(DataType.Text));
            Assert.That(domainObject.IsSystemField, Is.False);
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

            Dto.Caption = "Caption (Updated)";

            var domainObject = PropertyDefinitionAssembler.UpdateDomainObject(Dto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(1));
            Assert.That(domainObject.Version, Is.EqualTo(2));
            Assert.That(domainObject.Name, Is.EqualTo("Caption (Updated)"));
            Assert.That(domainObject.DataType, Is.EqualTo(DataType.Text));
        }

        [Test]
        public void UpdateDomainObject_does_not_update_IsSystemProperty()
        {
            GenerateAndRegisterMissingStubs();

            Dto.IsSystemProperty = false;

            var domain = PropertyDefinitionAssembler.UpdateDomainObject(Dto);
            Assert.That(domain, Is.Not.Null);
            Assert.That(domain.IsSystemField, Is.True);
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            FakePropertyDefRepo = GenerateAndRegisterStub<IFieldDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Get(1)).Return(null);

            Assert.Throws<EntityNotFoundException>(() => PropertyDefinitionAssembler.UpdateDomainObject(Dto));
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

            Dto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => PropertyDefinitionAssembler.UpdateDomainObject(Dto));
        }
    }
}