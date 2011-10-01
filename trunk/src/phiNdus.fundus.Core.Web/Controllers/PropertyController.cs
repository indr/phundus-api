using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        protected IPropertyService PropertyService
        {
            get { return IoC.Resolve<IPropertyService>(); }
        }

        //
        // GET: /Property/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Property/List

        public ActionResult List()
        {
            return View(PropertyService.GetProperties(Session.SessionID));
        }

        //
        // GET: /Property/Details/5
        public ActionResult Details(int id)
        {
            var model = new PropertyDefinitionViewModel(PropertyService.GetProperty(Session.SessionID, id));
            return View(model);
        }

        //
        // GET: /Property/Create
        public ActionResult Create()
        {
            return View(new PropertyDefinitionViewModel());
        }

        //
        // POST: /Property/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new PropertyDefinitionViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                PropertyService.CreateProperty(Session.SessionID, model.Item);
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
        // GET: /Property/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new PropertyDefinitionViewModel(PropertyService.GetProperty(Session.SessionID, id)));
        }

        //
        // POST: /Property/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new PropertyDefinitionViewModel(PropertyService.GetProperty(Session.SessionID, id));
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                PropertyService.UpdateProperty(Session.SessionID, model.Item);
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
        // GET: /Property/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new PropertyDefinitionViewModel(PropertyService.GetProperty(Session.SessionID, id)));
        }

        //
        // POST: /Property/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var model = new PropertyDefinitionViewModel(PropertyService.GetProperty(Session.SessionID, id));
            try
            {
                PropertyService.DeleteProperty(Session.SessionID, model.Item);
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