using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class ArticleAssemblerTests : BaseTestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            StubDomainPropertyDefinitionRepository = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();

            StubDomainPropertyDefinitionRepository.Expect(x => x.Get(DomainPropertyDefinition.CaptionId))
                .Return(new DomainPropertyDefinition(DomainPropertyDefinition.CaptionId, "Name", DomainPropertyType.Text));

            ArticleDto = new ArticleDto();
            ArticleDto.Id = 1;
            ArticleDto.Version = 2;

            Article = new Article(1, 2);
            Article.Caption = "Artikel";
        }

        protected IDomainPropertyDefinitionRepository StubDomainPropertyDefinitionRepository { get; set; }

        private Article Article { get; set; }
        private ArticleDto ArticleDto { get; set; }

        [Test]
        public void CreateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = ArticleAssembler.CreateDomainObject(ArticleDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.Version, Is.EqualTo(0));
        }

        [Test]
        public void CreateDto_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleAssembler.CreateDto(null));
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
    }
}