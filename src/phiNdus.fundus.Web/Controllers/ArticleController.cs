namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Helpers.FileUpload;
    using Microsoft.Practices.ServiceLocation;
    using Models.ArticleModels;
    using Phundus.Core.Inventory._Legacy.Services;
    using Phundus.Core.InventoryCtx.Model;
    using Phundus.Core.ReservationCtx.Repositories;
    using ViewModels;

    public class ArticleController : ControllerBase
    {
        private static string MasterView
        {
            get { return @"_Tabs"; }
        }

        protected IArticleService ArticleService
        {
            get { return ServiceLocator.Current.GetInstance<IArticleService>(); }
        }

        protected IFieldsService FieldsService
        {
            get { return ServiceLocator.Current.GetInstance<IFieldsService>(); }
        }


        [Transaction]
        public virtual ActionResult Index()
        {
            return RedirectToAction("list");
        }

        [Transaction]
        public virtual ActionResult List()
        {
            if (!CurrentOrganizationId.HasValue)
                throw new Exception("Keine Organisation ausgewählt.");

            var model = new ArticlesTableViewModel(
                ArticleService.GetArticles(CurrentOrganizationId.Value),
                ArticleService.GetProperties()
                );
            return View(model);
        }

        [Transaction]
        public virtual ActionResult Create()
        {
            var model = new ArticleViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Create(FormCollection collection)
        {
            var model = new ArticleViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                var articleId = ArticleService.CreateArticle(model.CreateDto(), CurrentOrganizationId.Value);
                return RedirectToAction("Images", new {id = articleId});
            }

            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View("Create", model);
            }
        }

        [Transaction]
        public virtual ActionResult Edit(int id)
        {
            return Fields(id);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Edit(int id, FormCollection collection)
        {
            return Fields(id, collection);
        }

        [Transaction]
        public virtual ActionResult Fields(int id)
        {
            var model = new ArticleViewModel(
                ArticleService.GetArticle(id),
                ArticleService.GetProperties());
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Fields, model);
            }
            return View(Views.Fields, MasterView, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Fields(int id, FormCollection collection)
        {
            var model = new ArticleViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                ArticleService.UpdateArticle(model.CreateDto(), CurrentOrganizationId.Value);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(Views.Fields, model);
            }
        }

        [Transaction]
        public virtual ActionResult Images(int id)
        {
            var model = new ArticleViewModel(id);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Images, model);
            return View(Views.Images, MasterView, model);
        }

        [AllowAnonymous]
        [Transaction]
        public virtual ActionResult ImageStore(int id, string name)
        {
            var path = String.Format(@"~\Content\Images\Articles\{0}", id);
            var store = new ImageStore(path);

            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = Url.Content(path);
            factory.DeleteUrl = Url.Action("ImageStore", "Article");

            var handler = new BlueImpFileUploadHandler(store);

            if (Request.HttpMethod == "POST")
            {
                var images = handler.Post(HttpContext.Request.Files);
                foreach (var each in images)
                {
                    ArticleService.AddImage(id, each, CurrentOrganizationId.Value);
                }
                var result = factory.Create(images);
                return Json(result);
            }
            if (Request.HttpMethod == "GET")
            {
                var images = ArticleService.GetImages(id);
                var result = factory.Create(images);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Request.HttpMethod == "DELETE")
            {
                var fileName = store.FilePath + Path.DirectorySeparatorChar + name;
                ArticleService.DeleteImage(id, fileName, CurrentOrganizationId.Value);
                store.Delete(name);
                return Json("");
            }
            return Json("");
        }

        [Transaction]
        public virtual ActionResult Availability(int id)
        {
            var model = new ArticleAvailabilityViewModel();
            model.Id = id;
            model.Availabilites = ServiceLocator.Current.GetInstance<IArticleService>().GetAvailability(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Availability, model);
            }
            return View(Views.Availability, MasterView, model);
        }

        [Transaction]
        public virtual ActionResult Reservations(int id)
        {
            var model = new ArticleReservationsModel();
            model.Items = ServiceLocator.Current.GetInstance<IReservationRepository>().Find(new Article(id, 0));
            if (Request.IsAjaxRequest())
                return PartialView(Views.Reservations, model);
            return View(Views.Reservations, MasterView, model);
        }

        [Transaction]
        public virtual ActionResult Categories(int id)
        {
            var model = new ArticleViewModel(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Categories, model);
            }
            return View(Views.Categories, MasterView, model);
        }

        [Transaction]
        public virtual ActionResult Delete(int id)
        {
            return View(new ArticleViewModel(id));
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Delete(int id, int version)
        {
            var model = new ArticleViewModel(id);
            try
            {
                model.Version = version;
                ArticleService.DeleteArticle(model.CreateDto(), CurrentOrganizationId.Value);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Transaction]
        public virtual ActionResult AjaxDelete(int id)
        {
            MessageBoxViewModel result;
            try
            {
                result = new MessageBoxViewModel
                {
                    Message = "Der Artikel wurde erfolgreich gelöscht.",
                    Type = MessageBoxType.Success
                };
            }
            catch (Exception ex)
            {
                result = new MessageBoxViewModel
                {
                    Message = ex.Message,
                    Type = MessageBoxType.Error
                };
            }
            return DisplayFor(result);
        }

        [HttpGet]
        [Transaction]
        public virtual ActionResult AddPropertyAjax(int id, string prefix)
        {
            var prop = FieldsService.GetField(id);
            var model = ArticleViewModel.ConvertToPropertyValueViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        [Transaction]
        public virtual ActionResult AddDiscriminatorAjax(int id, string prefix)
        {
            var prop = FieldsService.GetField(id);
            var model = ArticleViewModel.ConvertToDiscriminatorViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        [Transaction]
        public virtual ActionResult AddChild(string prefix)
        {
            var model = new ArticleViewModel();
            model.IsChild = true;
            return EditorFor(model, prefix);
        }

        #region Nested type: Views

        private static class Views
        {
            public static string Fields
            {
                get { return @"Fields"; }
            }

            public static string Images
            {
                get { return @"Images"; }
            }

            public static string Categories
            {
                get { return @"Categories"; }
            }

            public static string Availability
            {
                get { return @"Availability"; }
            }

            public static string Reservations
            {
                get { return @"Reservations"; }
            }
        }

        #endregion
    }
}