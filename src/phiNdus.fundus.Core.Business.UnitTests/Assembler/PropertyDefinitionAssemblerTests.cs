using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class PropertyDefinitionAssemblerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            PropertyDefinition = new DomainPropertyDefinition(1, "Caption", DomainPropertyType.Text);
        }

        #endregion

        protected DomainPropertyDefinition PropertyDefinition { get; set; }

        [Test]
        public void CreateDto_regarding_DataType()
        {
            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Boolean)).DataType,
                Is.EqualTo(PropertyDtoDataType.Boolean));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.DateTime)).DataType,
                Is.EqualTo(PropertyDtoDataType.DateTime));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Decimal)).DataType,
                Is.EqualTo(PropertyDtoDataType.Decimal));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Integer)).DataType,
                Is.EqualTo(PropertyDtoDataType.Integer));

            Assert.That(
                PropertyDefinitionAssembler.CreateDto(
                    new DomainPropertyDefinition(DomainPropertyType.Text)).DataType,
                Is.EqualTo(PropertyDtoDataType.Text));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = PropertyDefinitionAssembler.CreateDto(PropertyDefinition);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(0));
            Assert.That(dto.Caption, Is.EqualTo("Caption"));
            Assert.That(dto.DataType, Is.EqualTo(PropertyDtoDataType.Text));
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
    }
}