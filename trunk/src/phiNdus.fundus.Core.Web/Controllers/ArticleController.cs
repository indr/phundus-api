using System;
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
            public static string Availability { get { return @"Availability"; } }
            public static string Categories { get {return @"Categories"; } }
            public static string Details { get { return @"Details"; } }
        }

        protected IArticleService ArticleService { get { return IoC.Resolve<IArticleService>(); } }
        protected IPropertyService PropertyService { get { return IoC.Resolve<IPropertyService>(); } }

            //
        // GET: /Article/

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Article/List

        public ActionResult List()
        {
            var model = new ArticlesTableViewModel(
                ArticleService.GetArticles(Session.SessionID),
                ArticleService.GetProperties(Session.SessionID)
                );
            return View(model);
        }

        //
        // Get: /Article/Availability/5
        
        public ActionResult Availability(int id)
        {
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Availability, model);
            }
            else
            {
                return View(Views.Availability, MasterView, model);
            }
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(int id)
        {
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Details, model);
            }
            else
            {
                return View(Views.Details, MasterView, model);
            }
        }

        //
        // GET: /Article/Categories/5

        public ActionResult Categories(int id)
        {
            var model = new ArticleViewModel(ArticleService.GetArticle(Session.SessionID, id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Categories, model);
            }
            else
            {
                return View(Views.Categories, MasterView, model);
            }
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
            var properties = ArticleService.GetProperties(Session.SessionID);
            var model = new ArticleViewModel(properties);
            model.PropertyValues.Add(new PropertyValueViewModel
                                         {
                                             Caption = "Name",
                                             DataType = FieldDataType.Text,
                                             PropertyDefinitionId = 2
                                         });
            model.PropertyValues.Add(new PropertyValueViewModel
                                         {
                                             Caption = "Preis",
                                             DataType = FieldDataType.Decimal,
                                             PropertyDefinitionId = 4
                                         });
            model.Discriminators.Add(new DiscriminatorViewModel
                                         {
                                             Caption = "Grösse",
                                             PropertyDefinitionId = 9
                                         });
            
            var child1 = new ArticleViewModel(properties);
            child1.PropertyValues.Add(new PropertyValueViewModel
            {
                Caption = "Grösse",
                DataType = FieldDataType.Text,
                PropertyDefinitionId = 9,
                Value = "M"
            });
            child1.PropertyValues.Add(new PropertyValueViewModel
            {
                Caption = "Menge",
                DataType = FieldDataType.Integer,
                PropertyDefinitionId = 3
            });
            var child2 = new ArticleViewModel(properties);
            child2.PropertyValues.Add(new PropertyValueViewModel
            {
                Caption = "Grösse",
                DataType = FieldDataType.Text,
                PropertyDefinitionId = 9,
                Value = "L"
            });
            child2.PropertyValues.Add(new PropertyValueViewModel
            {
                Caption = "Menge",
                DataType = FieldDataType.Integer,
                PropertyDefinitionId = 3
            });
            model.Children.Add(child1);
            child1.IsChild = true;
            model.Children.Add(child2);
            child2.IsChild = true;

            return View(model);
        }

        //
        // POST: /Article/Create
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

        //
        // GET: /Article/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new ArticleViewModel(
                ArticleService.GetArticle(Session.SessionID, id),
                ArticleService.GetProperties(Session.SessionID));
            return View(model);
        }

        //
        // POST: /Article/Action/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        //
        // GET: /Article/Delete/5

        public ActionResult Delete(int id)
        {
            return View(new ArticleViewModel(
                            ArticleService.GetArticle(Session.SessionID, id)
                            ));
        }

        //
        // POST: /Article/Delete/5
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

        //
        // DELETE: /Article/Delete/5
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
            var prop = PropertyService.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToPropertyValueViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        public ActionResult AddDiscriminatorAjax(int id, string prefix)
        {
            var prop = PropertyService.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToDiscriminatorViewModel(prop);
            return EditorFor(model, prefix);
        }

        [HttpGet]
        public ActionResult AddChild(string prefix)
        {
            var model = new ArticleViewModel(PropertyService.GetProperties(Session.SessionID));
            model.IsChild = true;
            return EditorFor(model, prefix);
        }
    }
}