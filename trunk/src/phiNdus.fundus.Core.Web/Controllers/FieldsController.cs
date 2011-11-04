using System;
using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class FieldsController : ControllerBase
    {
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
            return View(new FieldsViewModel().Load());
        }

        //
        // POST: /Fields/List
        [HttpPost]
        public ActionResult List(IList<FieldViewModel> items)
        {
            var model = new FieldsViewModel().Load();
            try
            {
                if (model.Items.Count != items.Count)
                    throw new Exception("Daten sind veraltet :-P");

                for (var idx = 0; idx < items.Count; idx++)
                {
                    if ((model.Items[idx].Id != items[idx].Id) || (model.Items[idx].Version != items[idx].Version))
                        throw new Exception("Daten sind veraltet :-P");

                    model.Items[idx].IsDefault = items[idx].IsDefault;
                    model.Items[idx].Position = items[idx].Position;
                }

                model.Save();
                

                if (Request.IsAjaxRequest())
                    return DisplayFor(MessageBox.Success("Erfolgreich gespeichert."));
                ModelState.Clear();
                return View(new FieldsViewModel().Load());
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                    return DisplayFor(MessageBox.Error(ex.Message));
                
                ModelState.AddModelError("", ex.Message);
                return View(new FieldsViewModel().Load());
            }
        }

        //
        // GET: /Fields/Details/5
        public ActionResult Details(int id)
        {
            return View(new FieldViewModel().Load(id));
        }

        //
        // GET: /Fields/Create
        public ActionResult Create()
        {
            return View(new FieldViewModel());
        }

        //
        // POST: /Fields/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var model = new FieldViewModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                model.Save();
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
            return View(new FieldViewModel().Load(id));
        }

        //
        // POST: /Fields/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, int version, FormCollection collection)
        {
            var model = new FieldViewModel().Load(id, version);
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                model.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Fields/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new FieldViewModel().Load(id));
        }

        //
        // POST: /Fields/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int version)
        {
            var model = new FieldViewModel().Load(id, version);
            try
            {
                model.Delete();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}