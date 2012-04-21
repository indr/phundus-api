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

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(UserService.GetUsers(Session.SessionID));
        }

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