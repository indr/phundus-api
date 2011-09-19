using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    public class ArticleServiceTests : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new ArticleService();
        }

        protected ArticleService Sut { get; set; }

        [Test]
        public void Can_create_and_get_article()
        {
            var dto = new ArticleDto();
            dto.AddProperty(new DtoProperty
                                {
                                    PropertyId = DomainPropertyDefinition.CaptionId,
                                    Value = "Artikel"
                                });
            var id = Sut.CreateArticle(dto);

            dto = Sut.GetArticle(id);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.Version, Is.EqualTo(1));
            Assert.That(dto.Properties, Has.Count.EqualTo(1));
            Assert.That(dto.Properties, Has.Some.Property("Value").EqualTo("Artikel"));
        }
    }
}
