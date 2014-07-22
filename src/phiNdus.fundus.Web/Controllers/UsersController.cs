namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Microsoft.Practices.ServiceLocation;
    using Models;
    using Phundus.Core.IdentityAndAccessCtx.Mails;
    using Phundus.Core.IdentityAndAccessCtx.Queries;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;

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
            var user = ServiceLocator.Current.GetInstance<IUserRepository>().ById(id);
            if (user == null)
                return HttpNotFound();

            user.Membership.LockOut();
            SessionFact().Update(user);

            new UserLockedOutMail().For(user)
                .Send(user);
            //.Send(Settings.Common.AdminEmailAddress);


            return Json(id);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Unlock(int id)
        {
            var user = ServiceLocator.Current.GetInstance<IUserRepository>().ById(id);
            if (user == null)
                return HttpNotFound();

            user.Membership.Unlock();
            SessionFact().Update(user);

            new UserUnlockedMail().For(user)
                .Send(user);
            //.Send(Settings.Common.AdminEmailAddress);


            return Json(id);
        }
    }
}