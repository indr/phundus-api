using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.UnitTests.Assembler
{
    [TestFixture]
    public class ArticleDtoAssemblerTests: ArticleAssemblerTestBase
    {
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
    }
}