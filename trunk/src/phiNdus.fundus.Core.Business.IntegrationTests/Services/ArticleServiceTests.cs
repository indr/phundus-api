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
        public void Can_create_article()
        {
            // Create
            var dto = new ArticleDto();
            dto.AddProperty(new DtoProperty
                                {
                                    PropertyId = DomainPropertyDefinition.CaptionId,
                                    DataType =  DtoPropertyDataType.Text,
                                    Value = "Artikel"
                                });
            var id = Sut.CreateArticle(dto);

            // Read
            dto = Sut.GetArticle(id);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.Version, Is.EqualTo(1));
            Assert.That(dto.Properties, Has.Count.EqualTo(1));
            Assert.That(dto.Properties, Has.Some.Property("Value").EqualTo("Artikel"));
        }

        [Test]
        public void Can_update_article()
        {
            // Create
            var dto = new ArticleDto();
            dto.AddProperty(new DtoProperty
            {
                PropertyId = DomainPropertyDefinition.CaptionId,
                DataType = DtoPropertyDataType.Text,
                Value = "Artikel"
            });
            var id = Sut.CreateArticle(dto);

            // Update
            dto = Sut.GetArticle(id);
            dto.AddProperty(new DtoProperty
                                {
                                    PropertyId =  DomainPropertyDefinition.PriceId,
                                    DataType = DtoPropertyDataType.Decimal,
                                    Value = 12.50
                                });
            dto.RemoveProperty(DomainPropertyDefinition.CaptionId);
            Sut.UpdateArticle(dto);

            // Read
            dto = Sut.GetArticle(id);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.Properties, Has.Count.EqualTo(1));
            Assert.That(dto.Properties, Has.Some.Property("Value").EqualTo(12.50));
        }
    }
}
