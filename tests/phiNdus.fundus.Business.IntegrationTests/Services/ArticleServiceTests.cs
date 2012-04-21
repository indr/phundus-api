using System.Linq;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Business.IntegrationTests.Services
{
    [TestFixture]
    public class ArticleServiceTests : BusinessComponentTestBase<ArticleService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new ArticleService();
        }

        #endregion

        [Test]
        public void Can_create_article()
        {
            // Create
            var dto = new ArticleDto();
            dto.AddProperty(new FieldValueDto
                                {
                                    PropertyId = FieldDefinition.CaptionId,
                                    DataType = FieldDataType.Text,
                                    Value = "Artikel"
                                });
            var child = new ArticleDto();
            child.AddProperty(new FieldValueDto
                                  {
                                      PropertyId = FieldDefinition.CaptionId,
                                      DataType = FieldDataType.Text,
                                      Value = "Kind 1"
                                  });
            dto.AddChild(child);
            var id = Sut.CreateArticle(dto);

            // Read
            dto = Sut.GetArticle(id);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.Version, Is.EqualTo(1));
            Assert.That(dto.Properties, Has.Count.EqualTo(2));
            Assert.That(dto.Properties, Has.Some.Property("Value").EqualTo("Artikel"));

            Assert.That(dto.Children, Has.Count.EqualTo(1));
        }

        [Test]
        public void Can_update_article()
        {
            // Create
            var dto = new ArticleDto();
            dto.AddProperty(new FieldValueDto
                                {
                                    PropertyId = FieldDefinition.CaptionId,
                                    DataType = FieldDataType.Text,
                                    Value = "Artikel"
                                });
            var child1 = new ArticleDto();
            child1.AddProperty(new FieldValueDto
                                   {
                                       PropertyId = FieldDefinition.CaptionId,
                                       DataType = FieldDataType.Text,
                                       Value = "Kind 1"
                                   });
            dto.AddChild(child1);
            var child2 = new ArticleDto();
            child2.AddProperty(new FieldValueDto
                                   {
                                       PropertyId = FieldDefinition.CaptionId,
                                       DataType = FieldDataType.Text,
                                       Value = "Kind 2"
                                   });
            dto.AddChild(child2);
            var id = Sut.CreateArticle(dto);

            // Update
            dto = Sut.GetArticle(id);
            dto.AddProperty(new FieldValueDto
                                {
                                    PropertyId = FieldDefinition.PriceId,
                                    DataType = FieldDataType.Decimal,
                                    Value = 12.50
                                });
            dto.RemoveProperty(FieldDefinition.CaptionId);
            child1 =
                dto.Children.Where(
                    eachChild => eachChild.Properties.Any(eachProperty => eachProperty.ValueAsString == "Kind 1")).
                    FirstOrDefault();
            dto.RemoveChild(child1);
            var child3 = new ArticleDto();
            child3.AddProperty(new FieldValueDto
                                   {
                                       PropertyId = FieldDefinition.CaptionId,
                                       DataType = FieldDataType.Text,
                                       Value = "Kind 3"
                                   });
            dto.AddChild(child3);
            Sut.UpdateArticle(dto);

            // Read
            dto = Sut.GetArticle(id);
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Id, Is.EqualTo(id));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.Properties, Has.Count.EqualTo(2));
            Assert.That(dto.Properties, Has.Some.Property("Value").EqualTo(12.50));

            Assert.That(dto.Children, Has.Count.EqualTo(2));
            Assert.That(dto.Children.Where(
                eachChild => eachChild.Properties.Any(eachProperty => eachProperty.ValueAsString == "Kind 2")).
                            FirstOrDefault(), Is.Not.Null);
            Assert.That(dto.Children.Where(
                eachChild => eachChild.Properties.Any(eachProperty => eachProperty.ValueAsString == "Kind 3")).
                            FirstOrDefault(), Is.Not.Null);
        }
    }
}