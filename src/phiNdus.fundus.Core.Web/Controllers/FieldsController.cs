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
            return View(new FieldsViewModel());
        }

        //
        // POST: /Fields/List
        [HttpPost]
        public ActionResult List(IList<FieldDefinitionDto> items)
        {
            var model = new FieldsViewModel();
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
                
                FieldsService.UpdateFields(Session.SessionID, model.Items);

                if (Request.IsAjaxRequest())
                {
                    return DisplayFor(new MessageBoxViewModel
                                          {
                                              Message = "Erfolgreich gespeichert.",
                                              Type = MessageBoxType.Success
                                          });
                }
                else
                {
                    ModelState.Clear();
                    return View(new FieldsViewModel());
                }
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return DisplayFor(new MessageBoxViewModel
                    {
                        Message = ex.Message,
                        Type = MessageBoxType.Error
                    });
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(new FieldsViewModel());
                }
            }
        }

        //
        // GET: /Fields/Details/5
        public ActionResult Details(int id)
        {
            var model = new FieldViewModel(FieldsService.GetField(Session.SessionID, id));
            return View(model);
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
            return View(new FieldViewModel(FieldsService.GetField(Session.SessionID, id)));
        }

        //
        // POST: /Fields/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new FieldViewModel(FieldsService.GetField(Session.SessionID, id));
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
            return View(new FieldViewModel(FieldsService.GetField(Session.SessionID, id)));
        }

        //
        // POST: /Fields/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var model = new FieldViewModel(FieldsService.GetField(Session.SessionID, id));
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