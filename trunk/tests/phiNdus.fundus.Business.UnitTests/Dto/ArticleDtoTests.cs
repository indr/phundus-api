using NUnit.Framework;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.UnitTests.Dto
{
    [TestFixture]
    public class ArticleDtoTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new ArticleDto();
            Assert.That(Sut, Is.Not.Null);
        }

        #endregion

        protected ArticleDto Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new ArticleDto();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Is_derived_from_BaseDtoWithDynProperties()
        {
            var sut = new ArticleDto();
            Assert.That(sut, Is.InstanceOf(typeof(BasePropertiesDto)));
        }

        [Test]
        public void Can_get_and_set_Id()
        {
            Sut.Id = 1;
            Assert.That(Sut.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_Version()
        {
            Sut.Version = 1;
            Assert.That(Sut.Version, Is.EqualTo(1));
        }

        [Test]
        public void Can_add_and_remove_Child()
        {
            Assert.That(Sut.Children, Has.Count.EqualTo(0));
            var child = new ArticleDto();
            Sut.AddChild(child);
            Assert.That(Sut.Children, Has.Count.EqualTo(1));
            Sut.RemoveChild(child);
            Assert.That(Sut.Children, Has.Count.EqualTo(0));
        }
    }
}