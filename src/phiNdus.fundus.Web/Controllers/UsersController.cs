namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Business.Assembler;
    using Business.Mails;
    using Castle.Transactions;
    using Domain.Repositories;
    using Models;
    using piNuts.phundus.Infrastructure.Obsolete;

    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        public IUserRepository Users { get; set; }

        [Transaction]
        public virtual ActionResult Index()
        {
            var model = new UserAssembler().CreateDtos(Users.FindAll());
            return View(model);
        }

        [Transaction]
        public virtual ActionResult Edit(int id)
        {
            try
            {
                var model = new UserModel(new UserAssembler().CreateDto(Users.Get(id)));
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
            var userModel = new UserModel(new UserAssembler().CreateDto(Users.Get(id)));
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
        [Transaction]
        public virtual ActionResult Unlock(int id)
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