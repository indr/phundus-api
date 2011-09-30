using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    internal class MyHttpParamActionAttribute : ActionNameSelectorAttribute
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
            return View(new ArticleListModel());
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(int id)
        {
            return View(new ArticleViewModel());
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
            var model = new ArticleViewModel(
                    ArticleService.GetProperties(Session.SessionID)
                );
            
            return View(model);
        }

        //
        // POST: /Article/Create/AddProperty
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult AddProperty(int? id, FormCollection collection)
        //{
        //    var model = new ArticleViewModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddPropertyById(propertyId);
        //    ModelState.Clear();
        //    if (!id.HasValue)
        //    {
        //        return View("Create", model);
        //    }
        //    else
        //    {
        //        return View("Edit", model);
        //    }
        //}

        //
        // POST: /Article/Action/CreateAddDiscriminator
        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult AddDiscriminator(int? id, FormCollection collection)
        //{
        //    var model = new ArticleViewModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    var propertyId = Convert.ToInt32(collection["DropDownListRemainingProperties"]);
        //    model.AddDiscriminatorById(propertyId);
        //    ModelState.Clear();
        //    if (!id.HasValue)
        //    {
        //        return View("Create", model);
        //    }
        //    else
        //    {
        //        return View("Edit", model);
        //    }
        //}

        //
        // POST: /Article/Action/CreateRemoveProperty
        //[HttpPost]
        //[MyHttpParamAction]
        //public ActionResult RemoveProperty(int? id, int propertyId, FormCollection collection)
        //{
        //    var model = new ArticleViewModel();
        //    UpdateModel(model, collection.ToValueProvider());
        //    model.RemovePropertyById(propertyId);
        //    ModelState.Clear();
        //    if (!id.HasValue)
        //    {
        //        return View("Create", model);
        //    }
        //    else
        //    {
        //        return View("Edit", model);
        //    }
        //}


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
            var model = new ArticleViewModel();
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
        public ActionResult Edit(FormCollection collection)
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

        //[HttpPost]
        //[HttpParamAction]
        //public ActionResult CreateOrEdit(int? id, FormCollection collection)
        //{
        //    var model = new ArticleViewModel();
        //    try
        //    {
        //        UpdateModel(model, collection.ToValueProvider());
        //        if ((id.HasValue) && (id.Value > 0))
        //            model.Update();
        //        else
        //            model.Create();
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Logging
        //        // TODO: Exception-Handling
        //        ModelState.AddModelError("", ex.Message);
        //        if ((id.HasValue) && (id.Value > 0))
        //            return View("Edit", model);
        //        else
        //            return View("Create", model);
        //    }
        //}

        //
        // GET: /Article/Delete/5

        public ActionResult Delete(int id)
        {
            //return View(new ArticleViewModel(id));
            throw new NotImplementedException();
        }

        //
        // POST: /Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int version)
        {
            var model = new ArticleViewModel();
            try
            {
                model.Id = id;
                model.Version = version;
                ArticleService.DeleteArticle(Session.SessionID, model.CreateDto());
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult AddPropertyAjax(int id)
        {
            var svc = IoC.Resolve<IPropertyService>();
            var prop = svc.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToPropertyValueViewModel(prop);
            return EditorFor(model);
        }

        [HttpGet]
        public ActionResult AddDiscriminatorAjax(int id)
        {
            var svc = IoC.Resolve<IPropertyService>();
            var prop = svc.GetProperty(Session.SessionID, id);
            var model = ArticleViewModel.ConvertToDiscriminatorViewModel(prop);
            return EditorFor(model);
        }

        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult AjaxDelete(int id)
        {
            return PartialView("_Deleted", "Der Artikel sollte nun gelöscht sein :o)");
        }
    }
}