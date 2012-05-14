using System;
using System.Web.Mvc;
using phiNdus.fundus.Business.Mails;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using phiNdus.fundus.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public ActionResult LockOut(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = IoC.Resolve<IUserRepository>().Get(id);
                if (user == null)
                    return HttpNotFound();

                user.Membership.LockOut();
                UnitOfWork.CurrentSession.Update(user);

                new UserLockedOutMail().For(user)
                    .Send(user)
                    .Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return Json(id);
        }

        [HttpPost]
        public ActionResult Unlock(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = IoC.Resolve<IUserRepository>().Get(id);
                if (user == null)
                    return HttpNotFound();

                user.Membership.Unlock();
                UnitOfWork.CurrentSession.Update(user);

                new UserUnlockedMail().For(user)
                    .Send(user)
                    .Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return Json(id);
        }
    }
}