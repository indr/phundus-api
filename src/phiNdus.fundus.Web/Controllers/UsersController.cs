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
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private static IUserService UserService
        {
            get { return GlobalContainer.Resolve<IUserService>(); }
        }

        public ActionResult Index()
        {
            var model = UserService.GetUsers(Session.SessionID);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var model = new UserModel(id);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(UsersActionNames.Index);
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

                return RedirectToAction(UsersActionNames.Index);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userModel);
            }
        }

        [HttpPost]
        public ActionResult LockOut(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = GlobalContainer.Resolve<IUserRepository>().Get(id);
                if (user == null)
                    return HttpNotFound();

                user.Membership.LockOut();
                UnitOfWork.CurrentSession.Update(user);

                new UserLockedOutMail().For(user)
                    .Send(user);
                    //.Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return Json(id);
        }

        [HttpPost]
        public ActionResult Unlock(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var user = GlobalContainer.Resolve<IUserRepository>().Get(id);
                if (user == null)
                    return HttpNotFound();

                user.Membership.Unlock();
                UnitOfWork.CurrentSession.Update(user);

                new UserUnlockedMail().For(user)
                    .Send(user);
                    //.Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return Json(id);
        }
    }
}