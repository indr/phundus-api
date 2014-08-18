namespace Phundus.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Mails;
    using Core.IdentityAndAccess.Users.Repositories;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.Models;

    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        public IUserRepository Users { get; set; }

        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            var model = UserQueries.All();
            return View(model.ToArray());
        }

        [Transaction]
        public virtual ActionResult Edit(int id)
        {
            try
            {
                var model = new UserModel(UserQueries.ById(id));
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(UsersActionNames.Index);
            }
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Edit(int id, FormCollection collection)
        {
            var userModel = new UserModel(UserQueries.ById(id));
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
        [Transaction]
        public virtual ActionResult LockOut(int id)
        {
            var user = ServiceLocator.Current.GetInstance<IUserRepository>().GetById(id);
            if (user == null)
                return HttpNotFound();

            user.Account.LockOut();
            SessionFact().Update(user);

            new UserLockedOutMail().For(user)
                .Send(user);

            return Json(id);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Unlock(int id)
        {
            var user = ServiceLocator.Current.GetInstance<IUserRepository>().GetById(id);
            if (user == null)
                return HttpNotFound();

            user.Account.Unlock();
            SessionFact().Update(user);

            new UserUnlockedMail().For(user)
                .Send(user);

            return Json(id);
        }
    }
}