using System;
using System.Dynamic;
using System.IO;
using System.Web.Mvc;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Helpers;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private static string MasterView { get { return @"_Tabs"; } }

        private static class Views
        {
            public static string Fields { get { return @"Fields"; } }
            public static string Images { get { return @"Images"; } }
            public static string Categories { get { return @"Categories"; } }
            public static string Availability { get { return @"Availability"; } }
        }

        protected IArticleService ArticleService { get { return IoC.Resolve<IArticleService>(); } }
        protected IFieldsService FieldsService { get { return IoC.Resolve<IFieldsService>(); } }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new ArticlesTableViewModel(
                ArticleService.GetArticles(Session.SessionID),
                ArticleService.GetProperties(Session.SessionID)
                );
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ArticleViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ArticleViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                var articleId = ArticleService.CreateArticle(Session.SessionID, model.CreateDto());
                return RedirectToAction("Images", new { id = articleId });
            }

            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View("Create", model);
            }
        }


        public ActionResult Edit(int id)
        {
            return Fields(id);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            return Fields(id, collection);
        }

        public ActionResult Fields(int id)
        {
            var model = new ArticleViewModel(
                ArticleService.GetArticle(Session.SessionID, id),
                ArticleService.GetProperties(Session.SessionID));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Fields, model);
            }
            return View(Views.Fields, MasterView, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Fields(int id, FormCollection collection)
        {
            var model = new ArticleViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                ArticleService.UpdateArticle(Session.SessionID, model.CreateDto());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View("Edit", model);
            }
        }

        public ActionResult Images(int id)
        {
            var model = new ArticleViewModel(id);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Images, model);
            return View(Views.Images, MasterView, model);
        }

        public ActionResult ImageStore(int id, string name)
        {
            var path = String.Format(@"~\Content\Images\Articles\{0}", id);
            
            var store = new ImageStore();
            store.FilePath = path;
            
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = Url.Content(path);
            factory.DeleteUrl = Url.Action("ImageStore", "Article");

            var handler = new BlueImpFileUploadHandler(store);

            if (Request.HttpMethod == "POST")
            {
                var images = handler.Post(HttpContext.Request.Files);
                foreach (var each in images)
                {
                    ArticleService.AddImage(Session.SessionID, id, each);
                }
                var result = factory.Create(images);
                return Json(result);
            }
            if (Request.HttpMethod == "GET")
            {
                var images = ArticleService.GetImages(Session.SessionID, id);
                var result = factory.Create(images);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Request.HttpMethod == "DELETE")
            {
                var fileName = store.FilePath + Path.DirectorySeparatorChar + name;
                ArticleService.DeleteImage(Session.SessionID, id, fileName);
                store.Delete(name);
                return Json("");
            }
            return Json("");
        }

        public ActionResult Availability(int id)
        {
            var model = new ArticleAvailabilityViewModel();
            model.Id = id;
            model.Availabilites = IoC.Resolve<IArticleService>().GetAvailability(Session.SessionID, id);
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Availability, model);
            }
            return View(Views.Availability, MasterView, model);
        }

        public ActionResult Categories(int id)
        {
            var model = new ArticleViewModel(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Categories, model);
            }
            return View(Views.Categories, MasterView, model);
        }

        public ActionResult Delete(int id)
        {
            return View(new ArticleViewModel(id));
        }

        [HttpPost]
        public ActionResult Delete(int id, int version)
        {
            var model = new ArticleViewModel(id);
            try
            {
                model.Version = version;
                ArticleService.DeleteArticle(Session.SessionID, model.CreateDto());
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult AjaxDelete(int id)
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
        public ActionResult AddPropertyAjax(int id, string prefix)
        {
            var prop = FieldsService.GetField(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToPropertyValueViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        public ActionResult AddDiscriminatorAjax(int id, string prefix)
        {
            var prop = FieldsService.GetField(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToDiscriminatorViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        public ActionResult AddChild(string prefix)
        {
            var model = new ArticleViewModel();
            model.IsChild = true;
            return EditorFor(model, prefix);
        }

        
    }

    
}