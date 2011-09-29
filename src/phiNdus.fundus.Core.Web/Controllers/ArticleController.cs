using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.Models;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class MyHttpParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
                return false;

            var request = controllerContext.RequestContext.HttpContext.Request;
            var result = request[methodInfo.Name] != null;

            if (!result)
            {
                foreach (var each in request.Params.AllKeys)
                {
                    var match = Regex.Match(each,
                                            @"^" + methodInfo.Name + @"_(\d*)$",
                                            RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        controllerContext.RouteData.Values.Add("propertyId", match.Groups[1].Value);
                        return true;
                    }
                }
            }
            return result;
            /*var match = System.Text.RegularExpressions.Regex.Match(actionName, @"^(.*?)_(\d*)$");
            if (match.Success)
            {
                controllerContext.RouteData.Values.Add("propertyId", Convert.ToInt32(match.Captures[2].Value));
                actionName = match.Captures[1].Value;
                

                
            }
           */
        }
    }

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
            return View(new ArticleModel(id));
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
            var model = new ArticleModel();
            return View(model);
        }

        //
        // POST: /Article/Create/AddProperty
        [HttpPost]
        [HttpParamAction]
        public ActionResult AddProperty(int? id, FormCollection collection)
        {
            var model = new ArticleModel();
            UpdateModel(model, collection.ToValueProvider());
            var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
            model.AddPropertyById(propertyId);
            ModelState.Clear();
            if (!id.HasValue)
            {
                return View("Create", model);
            }
            else
            {
                return View("Edit", model);
            }
        }

        //
        // POST: /Article/Action/CreateAddDiscriminator
        [HttpPost]
        [HttpParamAction]
        public ActionResult AddDiscriminator(int? id, FormCollection collection)
        {
            var model = new ArticleModel();
            UpdateModel(model, collection.ToValueProvider());
            var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
            model.AddDiscriminatorById(propertyId);
            ModelState.Clear();
            if (!id.HasValue)
            {
                return View("Create", model);
            }
            else
            {
                return View("Edit", model);
            }
        }

        //
        // POST: /Article/Action/CreateRemoveProperty
        [HttpPost]
        [MyHttpParamAction]
        public ActionResult RemoveProperty(int? id, int propertyId, FormCollection collection)
        {
            var model = new ArticleModel();
            UpdateModel(model, collection.ToValueProvider());
            model.RemovePropertyById(propertyId);
            ModelState.Clear();
            if (!id.HasValue)
            {
                return View("Create", model);
            }
            else
            {
                return View("Edit", model);
            }
        }


        ////
        //// POST: /Article/Create/AddProperty
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult CreateAddProperty(FormCollection collection)
        //{
        //    var model = new ArticleCreateModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddPropertyById(propertyId);
        //    return View("Create", model);
        //}

        ////
        //// POST: /Article/Action/CreateAddDiscriminator
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult CreateAddDiscriminator(FormCollection collection)
        //{
        //    var model = new ArticleCreateModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddDiscriminatorById(propertyId);
        //    return View("Create", model);
        //}

        ////
        //// POST: /Article/Action/CreateRemoveProperty
        //[HttpPost]
        //[MyHttpParamAction]
        //public ActionResult CreateRemoveProperty(int propertyId, FormCollection collection)
        //{
        //    var model = new ArticleCreateModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    model.RemovePropertyById(propertyId);
        //    return View("Create", model);
        //}

        //
        // POST: /Article/Create
        [HttpPost]
        [HttpParamAction]
        public ActionResult Create(FormCollection collection)
        {
            var model = new ArticleModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                model.Create();
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
            return View(new ArticleModel(id));
        }

        ////
        //// POST: /Article/Action/EditAddProperty
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult EditAddProperty(int id, FormCollection collection)
        //{
        //    var model = new ArticleEditModel(id);
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddPropertyById(propertyId);
        //    return View("Edit", model);
        //}

        ////
        //// POST: /Article/Action/EditAddDiscriminator
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult EditAddDiscriminator(int id, FormCollection collection)
        //{
        //    var model = new ArticleEditModel(id);
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddDiscriminatorById(propertyId);
        //    return View("Edit", model);
        //}

        ////
        //// POST: /Article/Action/EditRemoveProperty
        //[HttpPost]
        //[MyHttpParamAction]
        //public ActionResult EditRemoveProperty(int id, int propertyId, FormCollection collection)
        //{
        //    var model = new ArticleEditModel(id);
        //    UpdateModel(model, collection.ToValueProvider());
        //    model.RemovePropertyById(propertyId);
        //    return View("Edit", model);
        //}

        //
        // POST: /Article/Action/5
        [HttpPost]
        [HttpParamAction]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new ArticleModel();
            try
            {
                UpdateModel(model, collection.ToValueProvider());
                model.Update();
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
            return View(new ArticleModel(id));
        }

        //
        // POST: /Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var model = new ArticleModel(id);
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

        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult AjaxDelete(int id)
        {
            return PartialView("_Deleted", "Der Artikel sollte nun gelöscht sein :o)");
        }
    }
}