using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
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
            return View(new ArticleListModel());
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(int id)
        {
            return View(new ArticleEditModel(id));
        }

        //
        // GET: /Article/Create

        public ActionResult Create()
        {
            return View(new ArticleCreateModel());
        }

        //
        // POST: /Article/Create

        [HttpPost]
        public ActionResult Create(string btnSubmit, FormCollection collection)
        {
            var model = new ArticleCreateModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());

                if (btnSubmit == "AddProperty")
                {
                    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
                    model.AddPropertyById(propertyId);
                    return View(model);
                }
                if (btnSubmit == "AddDiscriminator")
                {
                    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
                    model.AddDiscriminatorById(propertyId);
                    return View(model);
                }
                if (btnSubmit == "AddChild")
                {
                    return View(model);
                }
                if (btnSubmit.StartsWith("RemoveChild"))
                {
                    return View(model);
                }
                if(btnSubmit.StartsWith("RemoveProperty"))
                {
                    var propertyId = Convert.ToInt32(btnSubmit.Remove(0, 15));
                    model.RemovePropertyById(propertyId);
                    return View(model);
                }

                model.Create();
                return RedirectToAction("Index");
            }
            // TODO: Logging
            // TODO: Exception-Handling
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Article/Edit/5

        public ActionResult Edit(int id)
        {
            return View(new ArticleEditModel(id));
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, string btnSubmit, FormCollection collection)
        {
            var model = new ArticleEditModel(id);
            try
            {
                UpdateModel(model, collection.ToValueProvider());

                if (btnSubmit == "AddProperty")
                {
                    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
                    model.AddPropertyById(propertyId);
                    return View(model);
                }
                if (btnSubmit == "AddDiscriminator")
                {
                    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
                    model.AddDiscriminatorById(propertyId);
                    return View(model);
                }
                if (btnSubmit.StartsWith("RemoveProperty"))
                {
                    var propertyId = Convert.ToInt32(btnSubmit.Remove(0, 15));
                    model.RemovePropertyById(propertyId);
                    return View(model);
                }

                model.Update();
                return RedirectToAction("Index");
            }
            // TODO: Logging
            // TODO: Exception-Handling
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //
        // GET: /Article/Delete/5

        public ActionResult Delete(int id)
        {
            return View(new ArticleEditModel(id));
        }

        //
        // POST: /Article/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var model = new ArticleEditModel(id);
            try
            {
                model.Delete();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}