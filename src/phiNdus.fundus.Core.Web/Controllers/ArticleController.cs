using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
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
            var properties = ArticleService.GetProperties(Session.SessionID);
            var model = new ArticleViewModel(properties);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ArticleViewModel(
                ArticleService.GetProperties(Session.SessionID)
                );
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                ArticleService.CreateArticle(Session.SessionID, model.CreateDto());
                return RedirectToAction("Index");
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
        public ActionResult Fields(int id, FormCollection collection)
        {
            var model = new ArticleViewModel(
                    ArticleService.GetProperties(Session.SessionID)
                );
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
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
                return PartialView(Views.Images, model);
            return View(Views.Images, MasterView, model);
        }

        [HttpPost]
        public ActionResult Images(int id, object dummy)
        {
            var result = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
            foreach (string file in HttpContext.Request.Files)
            {
                var postedFile = HttpContext.Request.Files[file];
                var originalFileName = postedFile.FileName;
                
                // TODO: ???
                if (HttpContext.Request.Browser.Browser.ToUpper() == "IE")
                {
                    string[] files = originalFileName.Split(new char[] { '\\' });
                    originalFileName = files[files.Length - 1];
                }

                if (postedFile.ContentLength == 0)
                    continue;

                var contentPath = String.Format(@"~\Content\Images\Articles\{0}", id);
                var path = Server.MapPath(contentPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                
                var savedFileName = path + Path.DirectorySeparatorChar + originalFileName;
                postedFile.SaveAs(savedFileName);

                result.Add(new ViewDataUploadFilesResult()
                {
                    Thumbnail_url = Url.Content(contentPath + '\\' + originalFileName),
                    Name = originalFileName,
                    Length = postedFile.ContentLength,
                    Type = postedFile.ContentType
                });
            }
            return Json(result.ToArray());
        }

        public ActionResult Availability(int id)
        {
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Availability, model);
            }
            return View(Views.Availability, MasterView, model);
        }

        public ActionResult Categories(int id)
        {
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Categories, model);
            }
            return View(Views.Categories, MasterView, model);
        }

        public ActionResult Delete(int id)
        {
            return View(new ArticleViewModel(
                            ArticleService.GetArticle(Session.SessionID, id)
                            ));
        }

        [HttpPost]
        public ActionResult Delete(int id, int version)
        {
            var model = new ArticleViewModel(
                ArticleService.GetArticle(Session.SessionID, id)
                );
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
            MessageBoxViewModel result = null;
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
            var model = new ArticleViewModel(FieldsService.GetProperties(Session.SessionID));
            model.IsChild = true;
            return EditorFor(model, prefix);
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string Thumbnail_url { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }
}