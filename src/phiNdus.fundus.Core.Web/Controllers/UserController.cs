using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private static IUserService UserService
        {
            get { return IoC.Resolve<IUserService>(); }
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /User/List

        public ActionResult List()
        {
            return View(UserService.GetUsers(Session.SessionID));
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            try
            {
                return View(new UserModel(id));
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            try
            {
                return View(new UserModel(id));
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var userModel = new UserModel(id);
            try
            {
                UpdateModel(userModel, collection.ToValueProvider());
                userModel.Update();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(userModel);
            }
        }
    }
}