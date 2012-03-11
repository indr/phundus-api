using System;
using System.Linq;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class ArticleDomainAssemblerTests: ArticleAssemblerTestBase
    {
        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = ArticleDomainAssembler.CreateDomainObject(ArticleDto);

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
            var domainObject = ArticleDomainAssembler.CreateDomainObject(ArticleDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Children, Has.Count.EqualTo(2));
        }

        [Test]
        public void CreateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleDomainAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_returns_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Properties.First(x => x.PropertyId == FieldDefinition.CaptionId).Value =
                "Artikel (Updated)";
            ArticleDto.RemoveProperty(ArticleDto.Properties.First(x => x.PropertyId == FieldDefinition.PriceId));

            var updated = ArticleDomainAssembler.UpdateDomainObject(ArticleDto);

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

            var updated = ArticleDomainAssembler.UpdateDomainObject(ArticleDto);

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

            Assert.Throws<EntityNotFoundException>(() => ArticleDomainAssembler.UpdateDomainObject(ArticleDto));
        }

        [Test]
        public void UpdateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ArticleDomainAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();

            ArticleDto.Version = 3;
            Assert.Throws<DtoOutOfDateException>(() => ArticleDomainAssembler.UpdateDomainObject(ArticleDto));
        }
    }
}