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
        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

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
            var model = new ArticleListViewModel(
                ArticleService.GetArticles(Session.SessionID),
                ArticleService.GetProperties(Session.SessionID)
                );
            return View(model);
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(int id)
        {
            return View(new ArticleViewModel(
                            ArticleService.GetArticle(Session.SessionID, id)
                            ));
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
            var model = new ArticleViewModel(
                ArticleService.GetProperties(Session.SessionID)
                );
            model.PropertyValues.Add(new PropertyValueViewModel
                                         {
                                             Caption = "Name",
                                             DataType = PropertyDataType.Text,
                                             PropertyDefinitionId = 2
                                         });
            model.PropertyValues.Add(new PropertyValueViewModel
                                         {
                                             Caption = "Preis",
                                             DataType = PropertyDataType.Decimal,
                                             PropertyDefinitionId = 4
                                         });
            model.PropertyValues.Add(new PropertyValueViewModel
                                         {
                                             Caption = "Menge",
                                             DataType = PropertyDataType.Integer,
                                             PropertyDefinitionId = 3
                                         });
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
            var svc = IoC.Resolve<IPropertyService>();
            var prop = svc.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToPropertyValueViewModel(prop);
            ViewData.TemplateInfo.HtmlFieldPrefix = String.Format(prefix, TempSurrogateKey.Next);
            return EditorFor(model);
        }

        [HttpGet]
        public ActionResult AddDiscriminatorAjax(int id, string prefix)
        {
            var svc = IoC.Resolve<IPropertyService>();
            var prop = svc.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToDiscriminatorViewModel(prop);
            ViewData.TemplateInfo.HtmlFieldPrefix = String.Format(prefix, TempSurrogateKey.Next);
            return EditorFor(model);
        }
    }

    internal class TempSurrogateKey
    {
        private int next = 1001;

        public static int Next
        {
            get
            {
                var tempSurrogateKey = (TempSurrogateKey) HttpContext.Current.Session["TempSurrogateKey"];
                if (tempSurrogateKey == null)
                {
                    tempSurrogateKey = new TempSurrogateKey();
                    HttpContext.Current.Session["TempSurrogateKey"] = tempSurrogateKey;
                }
                return tempSurrogateKey.next++;
            }
        }
    }
}