using System;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;
using phiNdus.fundus.Core.Web.Models;
using System.Linq;

namespace phiNdus.fundus.Core.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService UserService {
            get { return IoC.Resolve<IUserService>(); }
        }

        private IRoleService RoleService {
            get { return IoC.Resolve<IRoleService>(); }
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
            //var userService = IoC.Resolve<IUserService>();
            //return View(userService.GetUsers(Session.SessionID));
            return View(this.UserService.GetUsers(Session.SessionID));
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            try
            {
                //var userService = IoC.Resolve<IUserService>();
                //return View(userService.GetUser(Session.SessionID, id));
                return View(this.UserService.GetUser(Session.SessionID, id));
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
            try
            {
                var user = this.UserService.GetUser(Session.SessionID, id);
                var roles = this.RoleService.GetRoles(Session.SessionID);

                return View(UserModel.FromDto(user, roles));
                //var userService = IoC.Resolve<IUserService>();
                //return View(userService.GetUser(Session.SessionID, id));
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
            //var userService = IoC.Resolve<IUserService>();
            //var user = userService.GetUser(Session.SessionID, id);
            var user = this.UserService.GetUser(Session.SessionID, id);
            var roles = this.RoleService.GetRoles(Session.SessionID);
            //var userModel = new UserModel();
            try
            {
                UpdateModel(user, collection.ToValueProvider());

                // Todo,jac(low): Review. Der Rollenname wird manuell aktualisiert
                // da das binding nur über die id läuft.
                user.RoleName = roles.Single(r => r.Id == user.RoleId).Name;

                this.UserService.UpdateUser(Session.SessionID, user);

                return RedirectToAction("Index");
            }
                // TODO: Logging
                // TODO: Exception-Handling
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(UserModel.FromDto(user, roles));
            }
        }
    }
}