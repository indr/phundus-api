using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Models;
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
            return View(PropertyService.GetProperty(Session.SessionID, id));
        }

        //
        // GET: /Property/Create

        public ActionResult Create()
        {
            return View(new PropertyViewModel());
        } 

        //
        // POST: /Property/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new PropertyViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                PropertyService.CreateProperty(Session.SessionID, model.Item);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
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
            return View(new PropertyViewModel(PropertyService.GetProperty(Session.SessionID, id)));
        }

        //
        // POST: /Property/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new PropertyViewModel(PropertyService.GetProperty(Session.SessionID, id));
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                PropertyService.UpdateProperty(Session.SessionID, model.Item);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
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
            return View(PropertyService.GetProperty(Session.SessionID, id));
        }

        //
        // POST: /Property/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
