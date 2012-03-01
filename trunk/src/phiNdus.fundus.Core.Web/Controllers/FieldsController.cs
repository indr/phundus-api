using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.ViewModels;

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
                    throw new Exception("Daten sind veraltet :-P (1)");

                foreach (var each in items)
                {
                    var item = model.Items.First(i => i.Id == each.Id);
                    item.IsDefault = each.IsDefault;
                    item.Position = each.Position;

                }

                //for (var idx = 0; idx < items.Count; idx++)
                //{
                //    if ((model.Items[idx].Id != items[idx].Id))
                //        throw new Exception("Daten sind veraltet :-P (2)");

                //    model.Items[idx].IsDefault = items[idx].IsDefault;
                //    model.Items[idx].Position = items[idx].Position;
                //}

                model.Save();

                model = new FieldsViewModel().Load();


                if (Request.IsAjaxRequest())
                    return DisplayFor(MessageBox.Success("Erfolgreich gespeichert."));
                //{
                //    var data = new List<object>();
                //    foreach (var each in model.Items)
                //        data.Add(new {id = each.Id, version = each.Version});
                //    return Json(new
                //                    {
                //                        message = @"<div class=""success"">Erfolgreich gespeichert (2).</div>",
                //                        data = data
                //                    });
                //}
                ModelState.Clear();
                return View(model);
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