using System;
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

            StubDomainPropertyDefinitionRepository = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();

            StubDomainPropertyDefinitionRepository.Expect(x => x.Get(DomainPropertyDefinition.CaptionId))
                .Return(new DomainPropertyDefinition(DomainPropertyDefinition.CaptionId, "Name",
                                                     DomainPropertyType.Text));
            StubDomainPropertyDefinitionRepository.Expect(x => x.Get(DomainPropertyDefinition.PriceId))
                .Return(new DomainPropertyDefinition(DomainPropertyDefinition.PriceId, "Preis",
                                                     DomainPropertyType.Decimal));

            ArticleDto = new ArticleDto();
            ArticleDto.Id = 1;
            ArticleDto.Version = 2;
            ArticleDto.AddProperty(new DtoProperty
                                       {
                                           Caption = "Name",
                                           DataType = DtoPropertyDataType.Text,
                                           PropertyId = DomainPropertyDefinition.CaptionId,
                                           Value = "Artikel"
                                       });
            ArticleDto.AddProperty(new DtoProperty
                                       {
                                           Caption = "Preis",
                                           DataType = DtoPropertyDataType.Text,
                                           PropertyId = DomainPropertyDefinition.PriceId,
                                           Value = 12.50
                                       });

            Article = new Article(1, 2);
            Article.Caption = "Artikel";
            Article.Price = 12.50;
        }

        #endregion

        protected IDomainPropertyDefinitionRepository StubDomainPropertyDefinitionRepository { get; set; }
        protected IArticleRepository FakeArticleRepo { get; set; }

        private Article Article { get; set; }
        private ArticleDto ArticleDto { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IArticleRepository>() == null) {
                FakeArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
                FakeArticleRepo.Expect(x => x.Get(1)).Return(Article);
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
        }

        [Test]
        public void CreateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDto_returns_dto()
        {
            var dto = ArticleAssembler.CreateDto(Article);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(Article.Id));
            Assert.That(dto.Version, Is.EqualTo(Article.Version));
            Assert.That(dto.Properties, Has.Some.Property("PropertyId").EqualTo(DomainPropertyDefinition.CaptionId)
                                            .And.Property("Value").EqualTo("Artikel"));
        }

        [Test]
        public void CreateDto_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.CreateDto(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
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
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => ArticleAssembler.UpdateDomainObject(ArticleDto));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Properties.First(x => x.PropertyId == DomainPropertyDefinition.CaptionId).Value =
                "Artikel (Updated)";
            ArticleDto.RemoveProperty(ArticleDto.Properties.First(x => x.PropertyId == DomainPropertyDefinition.PriceId));

            var updated = ArticleAssembler.UpdateDomainObject(ArticleDto);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated, Is.SameAs(Article));
            Assert.That(updated.Caption, Is.EqualTo("Artikel (Updated)"));
            Assert.That(updated.Price, Is.EqualTo(0));
        }
    }
}