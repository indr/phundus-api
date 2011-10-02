using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

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


        private static DtoProperty CreateNameProperty(string value)
        {
            return new DtoProperty
                       {
                           Caption = "Name",
                           DataType = PropertyDataType.Text,
                           PropertyId = 2,
                           Value = value
                       };
        }

        private static DtoProperty CreatePreisProperty(double value)
        {
            return new DtoProperty
                       {
                           Caption = "Preis",
                           DataType = PropertyDataType.Decimal,
                           PropertyId = 4,
                           Value = value
                       };
        }

        private static DtoProperty CreateMengeProperty(int value)
        {
            return new DtoProperty
                       {
                           Caption = "Menge",
                           DataType = PropertyDataType.Integer,
                           PropertyId = 3,
                           Value = value
                       };
        }

        private static DtoProperty CreateReservierbarProperty(bool value)
        {
            return new DtoProperty
            {
                Caption = "Reservierbar",
                DataType = PropertyDataType.Boolean,
                PropertyId = 6,
                Value = value
            };
        }

        private static DtoProperty CreateAusleihbarProperty(bool value)
        {
            return new DtoProperty
            {
                Caption = "Ausleihbar",
                DataType = PropertyDataType.Boolean,
                PropertyId = 7,
                Value = value
            };
        }

        private ArticlesViewModel CreateTestArticlesViewModel()
        {
            var articleDtos = new List<ArticleDto>();

            ArticleDto article;

            article = new ArticleDto();
            article.AddProperty(CreateNameProperty("Schraube (10 Stk)"));
            article.AddProperty(CreatePreisProperty(2.50));
            article.AddProperty(CreateMengeProperty(6));
            article.AddProperty(CreateReservierbarProperty(true));
            article.AddProperty(CreateAusleihbarProperty(true));
            articleDtos.Add(article);

            article = new ArticleDto();
            article.AddProperty(CreateNameProperty("Canon IXUS 100IS"));
            article.AddProperty(CreatePreisProperty(49.90));
            article.AddProperty(CreateReservierbarProperty(true));
            article.AddProperty(CreateAusleihbarProperty(false));
            // Hier fehlt mindestens eine Diskriminante: z.B. Inventarcode
            // und natürlich Kinder, aber eine Mengenangabe ist eigentlich überflüssig, bzw. = 1
            articleDtos.Add(article);
            
            article = new ArticleDto();
            article.AddProperty(CreateNameProperty("Pullover"));
            article.AddProperty(CreatePreisProperty(29.90));
            article.AddProperty(CreateReservierbarProperty(false));
            article.AddProperty(CreateAusleihbarProperty(false));
            // Hier fehlt mindestens eine Diskriminante: z.B. Inventarcode, sicher
            // aber auch z.B. Farbe und Grösse.
            // ... und natürlich Kinder die entsprechenden Kinder mit der Menge.
            articleDtos.Add(article);

            

            return new ArticlesViewModel(
                articleDtos,
                ArticleService.GetProperties(Session.SessionID)
                );
        }
    }
}