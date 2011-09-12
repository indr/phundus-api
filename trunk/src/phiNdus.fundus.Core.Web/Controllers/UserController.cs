using System;
using System.Data;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
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
            var userService = IoC.Resolve<IUserService>();
            return View(userService.GetUsers(Session.SessionID));
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            try {
                var userService = IoC.Resolve<IUserService>();
                return View(userService.GetUser(Session.SessionID, id));
                }
            // TODO: Logging
            // TODO: Exception-Handling
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            try {
                var userService = IoC.Resolve<IUserService>();
                return View(userService.GetUser(Session.SessionID, id));
            }
            // TODO: Logging
            // TODO: Exception-Handling
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var userService = IoC.Resolve<IUserService>();
            var user = userService.GetUser(Session.SessionID, id);
            try
            {
                UpdateModel(user, collection.ToValueProvider());
                userService.UpdateUser(Session.SessionID, user);
                return RedirectToAction("Index");
            }
            // TODO: Logging
            // TODO: Exception-Handling
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }
    }
}