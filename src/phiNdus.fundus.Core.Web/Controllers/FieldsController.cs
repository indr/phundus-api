using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class FieldsController : Controller
    {
        protected IFieldsService FieldsService
        {
            get { return IoC.Resolve<IFieldsService>(); }
        }

        //
        // GET: /Fields/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Fields/List

        public ActionResult List()
        {
            return View(FieldsService.GetProperties(Session.SessionID));
        }

        //
        // GET: /Fields/Details/5
        public ActionResult Details(int id)
        {
            var model = new FieldDefinitionViewModel(FieldsService.GetField(Session.SessionID, id));
            return View(model);
        }

        //
        // GET: /Fields/Create
        public ActionResult Create()
        {
            return View(new FieldDefinitionViewModel());
        }

        //
        // POST: /Fields/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new FieldDefinitionViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                FieldsService.CreateField(Session.SessionID, model.Item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Fields/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new FieldDefinitionViewModel(FieldsService.GetField(Session.SessionID, id)));
        }

        //
        // POST: /Fields/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new FieldDefinitionViewModel(FieldsService.GetField(Session.SessionID, id));
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                FieldsService.UpdateField(Session.SessionID, model.Item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Fields/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new FieldDefinitionViewModel(FieldsService.GetField(Session.SessionID, id)));
        }

        //
        // POST: /Fields/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var model = new FieldDefinitionViewModel(FieldsService.GetField(Session.SessionID, id));
            try
            {
                FieldsService.DeleteField(Session.SessionID, model.Item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}