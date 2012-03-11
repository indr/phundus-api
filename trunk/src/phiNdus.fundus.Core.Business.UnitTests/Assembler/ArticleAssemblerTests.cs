using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ArticleAssemblerTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            StubFieldDefinitionRepository = GenerateAndRegisterStub<IFieldDefinitionRepository>();

            _captionPropertyDefinition = new FieldDefinition(FieldDefinition.CaptionId, "Name",
                                                                      DataType.Text);
            _pricePropertyDefinition = new FieldDefinition(FieldDefinition.PriceId, "Preis",
                                                                    DataType.Decimal);
            _colorPropertyDefinition = new FieldDefinition(101, "Farbe", DataType.Text);

            StubFieldDefinitionRepository.Expect(x => x.Get(FieldDefinition.CaptionId))
                .Return(_captionPropertyDefinition);
            StubFieldDefinitionRepository.Expect(x => x.Get(FieldDefinition.PriceId))
                .Return(_pricePropertyDefinition);
            StubFieldDefinitionRepository.Expect(x => x.Get(101)).Return(_colorPropertyDefinition);

            ArticleDto = new ArticleDto();
            ArticleDto.Id = 1;
            ArticleDto.Version = 2;
            ArticleDto.AddProperty(new FieldValueDto
                                       {
                                           Caption = "Name",
                                           DataType = FieldDataType.Text,
                                           PropertyId = FieldDefinition.CaptionId,
                                           Value = "Artikel"
                                       });
            ArticleDto.AddProperty(new FieldValueDto
                                       {
                                           Caption = "Preis",
                                           DataType = FieldDataType.Text,
                                           PropertyId = FieldDefinition.PriceId,
                                           Value = 12.50,
                                       });
            ArticleDto.AddProperty(new FieldValueDto
                                       {
                                           Caption = "Farbe",
                                           DataType = FieldDataType.Text,
                                           PropertyId = 101,
                                           IsDiscriminator = true
                                       });

            ChildDto1 = new ArticleDto();
            ChildDto1.Id = 2;
            ChildDto1.Version = 2;
            ChildDto1.AddProperty(new FieldValueDto
                                      {
                                          Caption = "Name",
                                          DataType = FieldDataType.Text,
                                          PropertyId = FieldDefinition.CaptionId,
                                          Value = "Child 1"
                                      });
            ChildDto1.AddProperty(new FieldValueDto
            {
                Caption = "Farbe",
                DataType = FieldDataType.Text,
                PropertyId = 101,
                Value = "Rot"
            });
            ArticleDto.AddChild(ChildDto1);

            ChildDto2 = new ArticleDto();
            ChildDto2.Id = 3;
            ChildDto2.Version = 2;
            ChildDto2.AddProperty(new FieldValueDto
                                      {
                                          Caption = "Name",
                                          DataType = FieldDataType.Text,
                                          PropertyId = FieldDefinition.CaptionId,
                                          Value = "Child 2"
                                      });
            ChildDto2.AddProperty(new FieldValueDto
            {
                Caption = "Farbe",
                DataType = FieldDataType.Text,
                PropertyId = 101,
                Value = "Blau"
            });
            ArticleDto.AddChild(ChildDto2);

            Article = new Article(1, 2);
            Article.Caption = "Artikel";
            Article.AddField(_colorPropertyDefinition).IsDiscriminator = true;
            Article.Price = 12.50;

            Child1 = new Article(2, 2);
            Child1.Caption = "Child 1";
            Child1.AddField(_colorPropertyDefinition, "Rot");
            Article.AddChild(Child1);

            Child2 = new Article(3, 2);
            Child2.Caption = "Child 2";
            Child2.AddField(_colorPropertyDefinition, "Blau");
            Article.AddChild(Child2);
        }

        #endregion

        private FieldDefinition _pricePropertyDefinition;
        private FieldDefinition _captionPropertyDefinition;
        private FieldDefinition _colorPropertyDefinition;

        protected IFieldDefinitionRepository StubFieldDefinitionRepository { get; set; }
        protected IArticleRepository FakeArticleRepo { get; set; }

        private Article Article { get; set; }
        private Article Child1 { get; set; }
        private Article Child2 { get; set; }
        private ArticleDto ArticleDto { get; set; }
        private ArticleDto ChildDto1 { get; set; }
        private ArticleDto ChildDto2 { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IArticleRepository>() == null)
            {
                FakeArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
                FakeArticleRepo.Expect(x => x.Get(1)).Return(Article);
                FakeArticleRepo.Expect(x => x.Get(2)).Return(Child1);
                FakeArticleRepo.Expect(x => x.Get(3)).Return(Child2);
            }
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = ArticleAssembler.CreateDomainObject(ArticleDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.Version, Is.EqualTo(0));
            Assert.That(domainObject.Caption, Is.EqualTo("Artikel"));
            Assert.That(domainObject.Price, Is.EqualTo(12.50));
            Assert.That(domainObject.FieldValues, Has.Some.Property("IsDiscriminator").EqualTo(true));
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object_with_children()
        {
            var domainObject = ArticleAssembler.CreateDomainObject(ArticleDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Children, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDto_assembles_children()
        {
            var dto = new ArticleDtoAssembler().CreateDto(Article);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Children, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = new ArticleDtoAssembler().CreateDto(Article);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(Article.Id));
            Assert.That(dto.Version, Is.EqualTo(Article.Version));
            Assert.That(dto.Properties, Has.Some.Property("PropertyId").EqualTo(FieldDefinition.CaptionId)
                                            .And.Property("Value").EqualTo("Artikel"));
            Assert.That(dto.Properties, Has.Some.Property("PropertyId").EqualTo(101)
                                            .And.Property("IsDiscriminator").EqualTo(true));
        }

        [Test]
        public void CreateDto_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ArticleDtoAssembler().CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDtos_returns_dtos()
        {
            var articles = new List<Article>();
            articles.Add(Article);
            articles.Add(Article);

            var dtos = new ArticleDtoAssembler().CreateDtos(articles);

            Assert.That(dtos, Is.Not.Null);
            Assert.That(dtos, Has.Length.EqualTo(2));
        }

        [Test]
        public void CreateDtos_with_subjects_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ArticleDtoAssembler().CreateDtos(null));
            Assert.That(ex.ParamName, Is.EqualTo("subjects"));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Properties.First(x => x.PropertyId == FieldDefinition.CaptionId).Value =
                "Artikel (Updated)";
            ArticleDto.RemoveProperty(ArticleDto.Properties.First(x => x.PropertyId == FieldDefinition.PriceId));

            var updated = ArticleAssembler.UpdateDomainObject(ArticleDto);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated, Is.SameAs(Article));
            Assert.That(updated.Caption, Is.EqualTo("Artikel (Updated)"));
            Assert.That(updated.Price, Is.EqualTo(0));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object_with_children()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.RemoveChild(ChildDto1);
            ArticleDto.AddChild(new ArticleDto());

            var updated = ArticleAssembler.UpdateDomainObject(ArticleDto);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Children, Has.Count.EqualTo(2));
            Assert.That(updated.Children, Has.Some.Property("Id").EqualTo(3));
            Assert.That(updated.Children, Has.Some.Property("Id").EqualTo(0));
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            FakeArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Expect(x => x.Get(1)).Return(null);

            Assert.Throws<EntityNotFoundException>(() => ArticleAssembler.UpdateDomainObject(ArticleDto));
        }

        [Test]
        public void UpdateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => ArticleAssembler.UpdateDomainObject(ArticleDto));
        }
    }
}