using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.Assembler
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    [TestFixture]
    public class FieldDefinitionAssemblerTests : UnitTestBase<object>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Domain = new FieldDefinition(1, 2, "Caption", DataType.Text, true);
            Domain.Position = 3;
            Domain.IsDefault = true;

            Dto = new FieldDefinitionDto();
            Dto.Id = 1;
            Dto.Version = 2;
            Dto.Caption = "Caption";
            Dto.DataType = FieldDataType.Text;
            Dto.Position = 3;
            Dto.IsDefault = true;

        }

        #endregion

        protected FieldDefinitionDto Dto { get; set; }
        protected FieldDefinition Domain { get; set; }

        protected IFieldDefinitionRepository FakePropertyDefRepo { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (GlobalContainer.TryResolve<IFieldDefinitionRepository>() == null)
            {
                FakePropertyDefRepo = GenerateAndRegisterStub<IFieldDefinitionRepository>();
                FakePropertyDefRepo.Expect(x => x.Get(1)).Return(Domain);
            }
        }

        [Test]
        public void CreateDto_regarding_DataType()
        {
            Assert.That(
                FieldDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Boolean)).DataType,
                Is.EqualTo(FieldDataType.Boolean));

            Assert.That(
                FieldDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.DateTime)).DataType,
                Is.EqualTo(FieldDataType.DateTime));

            Assert.That(
                FieldDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Decimal)).DataType,
                Is.EqualTo(FieldDataType.Decimal));

            Assert.That(
                FieldDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Integer)).DataType,
                Is.EqualTo(FieldDataType.Integer));

            Assert.That(
                FieldDefinitionAssembler.CreateDto(
                    new FieldDefinition(DataType.Text)).DataType,
                Is.EqualTo(FieldDataType.Text));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = FieldDefinitionAssembler.CreateDto(Domain);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.Caption, Is.EqualTo("Caption"));
            Assert.That(dto.DataType, Is.EqualTo(FieldDataType.Text));
            Assert.That(dto.IsSystem, Is.True);
            Assert.That(dto.IsDefault, Is.True);
            Assert.That(dto.Position, Is.EqualTo(3));
        }

        [Test]
        public void CreateDto_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => FieldDefinitionAssembler.CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDtos_returns_array()
        {
            var propertyDefinitions = new List<FieldDefinition>();
            propertyDefinitions.Add(new FieldDefinition(1, "Name 1", DataType.Boolean));
            propertyDefinitions.Add(new FieldDefinition(2, "Name 2", DataType.Text));
            var dtos = FieldDefinitionAssembler.CreateDtos(propertyDefinitions);

            Assert.That(dtos, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateDtos_with_subjects_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => FieldDefinitionAssembler.CreateDtos(null));
            Assert.That(ex.ParamName, Is.EqualTo("subjects"));
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = FieldDefinitionAssembler.CreateDomainObject(
                    new FieldDefinitionDto
                        {
                            Id = 1,
                            Version = 2,
                            Caption = "Caption",
                            DataType = FieldDataType.Text,
                            IsSystem = true,
                            IsDefault = true,
                            Position = 3
                        }
                );

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.Version, Is.EqualTo(0));
            Assert.That(domainObject.Name, Is.EqualTo("Caption"));
            Assert.That(domainObject.DataType, Is.EqualTo(DataType.Text));
            Assert.That(domainObject.IsSystem, Is.False);
            Assert.That(domainObject.IsDefault, Is.True);
            Assert.That(domainObject.Position, Is.EqualTo(3));
        }

        [Test]
        public void CreateDomainObject_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => FieldDefinitionAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            Dto.Caption = "Caption (Updated)";

            var domainObject = FieldDefinitionAssembler.UpdateDomainObject(Dto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(1));
            Assert.That(domainObject.Version, Is.EqualTo(2));
            Assert.That(domainObject.Name, Is.EqualTo("Caption (Updated)"));
            Assert.That(domainObject.DataType, Is.EqualTo(DataType.Text));
            Assert.That(domainObject.IsDefault, Is.EqualTo(true));
            Assert.That(domainObject.Position, Is.EqualTo(3));
        }

        [Test]
        public void UpdateDomainObject_does_not_update_IsSystem()
        {
            GenerateAndRegisterMissingStubs();

            Dto.IsSystem = false;

            var domain = FieldDefinitionAssembler.UpdateDomainObject(Dto);
            Assert.That(domain, Is.Not.Null);
            Assert.That(domain.IsSystem, Is.True);
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            FakePropertyDefRepo = GenerateAndRegisterStub<IFieldDefinitionRepository>();
            GenerateAndRegisterMissingStubs();

            FakePropertyDefRepo.Expect(x => x.Get(1)).Return(null);

            Assert.Throws<EntityNotFoundException>(() => FieldDefinitionAssembler.UpdateDomainObject(Dto));
        }

        [Test]
        public void UpdateDomainObject_with_subject_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => FieldDefinitionAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));   
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();

            Dto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => FieldDefinitionAssembler.UpdateDomainObject(Dto));
        }
    }
}