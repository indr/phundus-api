using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;
using System.Linq;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class ShopController : ControllerBase
    {
        private static IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        //
        // GET: /Shop/

        public ActionResult Index()
        {
            var model = CreateTestArticlesViewModel();
            return View(model);
        }

        //
        // GET: /Shop/Details/ArticleId
        public ActionResult Details(int id)
        {
            var model = CreateTestArticlesViewModel();

            var article = model.Articles.Single(a => a.Id == id);

            return View(article);
        }


        private static FieldValueDto CreateNameProperty(string value)
        {
            return new FieldValueDto
                       {
                           Caption = "Name",
                           DataType = FieldDataType.Text,
                           PropertyId = 2,
                           Value = value
                       };
        }

        private static FieldValueDto CreatePreisProperty(double value)
        {
            return new FieldValueDto
                       {
                           Caption = "Preis",
                           DataType = FieldDataType.Decimal,
                           PropertyId = 4,
                           Value = value
                       };
        }

        private static FieldValueDto CreateMengeProperty(int value)
        {
            return new FieldValueDto
                       {
                           Caption = "Menge",
                           DataType = FieldDataType.Integer,
                           PropertyId = 3,
                           Value = value
                       };
        }

        private static FieldValueDto CreateReservierbarProperty(bool value)
        {
            return new FieldValueDto
            {
                Caption = "Reservierbar",
                DataType = FieldDataType.Boolean,
                PropertyId = 6,
                Value = value
            };
        }

        private static FieldValueDto CreateAusleihbarProperty(bool value)
        {
            return new FieldValueDto
            {
                Caption = "Ausleihbar",
                DataType = FieldDataType.Boolean,
                PropertyId = 7,
                Value = value
            };
        }

        private static FieldValueDto CreateFarbeProperty(string value, bool discriminator)
        {
            return new FieldValueDto
            {
                Caption = "Farbe",
                DataType = FieldDataType.Text,
                PropertyId = 8,
                Value = value,
                IsDiscriminator = discriminator
            };
        }

        private static FieldValueDto CreateGrösseProperty(string value, bool discriminator)
        {
            return new FieldValueDto
            {
                Caption = "Grösse",
                DataType = FieldDataType.Text,
                PropertyId = 9,
                Value = value,
                IsDiscriminator = discriminator
            };
        }

        internal ArticlesViewModel CreateTestArticlesViewModel()
        {
            var articleDtos = new List<ArticleDto>();

            ArticleDto article;

            article = new ArticleDto();
            article.Id = 1;
            article.AddProperty(CreateNameProperty("Schraube (10 Stk)"));
            article.AddProperty(CreatePreisProperty(2.50));
            article.AddProperty(CreateMengeProperty(6));
            article.AddProperty(CreateReservierbarProperty(true));
            article.AddProperty(CreateAusleihbarProperty(true));
            articleDtos.Add(article);

            article = new ArticleDto();
            article.Id = 2;
            article.AddProperty(CreateNameProperty("Canon IXUS 100IS"));
            article.AddProperty(CreatePreisProperty(49.90));
            article.AddProperty(CreateReservierbarProperty(true));
            article.AddProperty(CreateAusleihbarProperty(false));
            // Hier fehlt mindestens eine Diskriminante: z.B. Inventarcode
            // und natürlich Kinder, aber eine Mengenangabe ist eigentlich überflüssig, bzw. = 1
            articleDtos.Add(article);

            article = new ArticleDto();
            article.Id = 3;
            article.AddProperty(CreateNameProperty("Pullover"));
            article.AddProperty(CreatePreisProperty(29.90));
            article.AddProperty(CreateReservierbarProperty(false));
            article.AddProperty(CreateAusleihbarProperty(false));
            // Hier fehlt mindestens eine Diskriminante: z.B. Inventarcode, sicher
            // aber auch z.B. Farbe und Grösse.
            article.AddProperty(CreateFarbeProperty(null, true));
            article.AddProperty(CreateGrösseProperty(null, true));
            // ... und natürlich Kinder die entsprechenden Kinder mit der Menge.
            article.Children.Add(CreateChildPullover(4, "rot", "S", 23));
            article.Children.Add(CreateChildPullover(5, "rot", "M", 21));
            article.Children.Add(CreateChildPullover(6, "rot", "L", 12));
            article.Children.Add(CreateChildPullover(7, "blau", "M", 19));
            article.Children.Add(CreateChildPullover(8, "blau", "L", 13));
            article.Children.Add(CreateChildPullover(9, "grün", "XS", 5));
            article.Children.Add(CreateChildPullover(10, "grün", "XL", 11));

            articleDtos.Add(article);


            return new ArticlesViewModel(
                articleDtos,
                ArticleService.GetProperties(Session.SessionID)
                );
        }

        private ArticleDto CreateChildPullover(int id, string farbe, string grösse, int menge)
        {
            var article = new ArticleDto();
            article.Id = id;

            article.AddProperty(CreateNameProperty(string.Format("Pullover ({0}, {1})", farbe, grösse)));
            article.AddProperty(CreateGrösseProperty(grösse, false));
            article.AddProperty(CreateFarbeProperty(farbe, false));
            article.AddProperty(CreateMengeProperty(menge));

            return article;
        }
    }
}