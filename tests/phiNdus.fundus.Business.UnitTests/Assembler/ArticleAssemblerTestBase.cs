using System;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.Assembler
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    public class ArticleAssemblerTestBase : UnitTestBase<object>
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
            _createDatePropertyDefinition = new FieldDefinition(FieldDefinition.CreateDateId, "Erfassungsdatum",
                                                            DataType.DateTime);
            _colorPropertyDefinition = new FieldDefinition(101, "Farbe", DataType.Text);
            StubFieldDefinitionRepository.Expect(x => x.Get(FieldDefinition.CaptionId))
                .Return(_captionPropertyDefinition);
            StubFieldDefinitionRepository.Expect(x => x.Get(FieldDefinition.PriceId))
                .Return(_pricePropertyDefinition);
            StubFieldDefinitionRepository.Expect(x => x.Get(FieldDefinition.CreateDateId))
                .Return(_createDatePropertyDefinition);
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
        private FieldDefinition _createDatePropertyDefinition;

        protected IFieldDefinitionRepository StubFieldDefinitionRepository { get; set; }
        protected IArticleRepository FakeArticleRepo { get; set; }

        protected Article Article { get; set; }
        protected Article Child1 { get; set; }
        protected Article Child2 { get; set; }
        protected ArticleDto ArticleDto { get; set; }
        protected ArticleDto ChildDto1 { get; set; }
        protected ArticleDto ChildDto2 { get; set; }

        protected void GenerateAndRegisterMissingStubs()
        {
            if (GlobalContainer.TryResolve<IArticleRepository>() == null)
            {
                FakeArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
                FakeArticleRepo.Expect(x => x.Get(1)).Return(Article);
                FakeArticleRepo.Expect(x => x.Get(2)).Return(Child1);
                FakeArticleRepo.Expect(x => x.Get(3)).Return(Child2);
            }
        }
    }
}