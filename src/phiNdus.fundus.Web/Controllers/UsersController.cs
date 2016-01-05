namespace Phundus.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Repositories;
    using phiNdus.fundus.Web.Models;

    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        public IUserRepository UserRepository { get; set; }

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
                var model = new UserModel(UserQueries.GetById(id));
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
            throw new NotImplementedException();
            //var userModel = new UserModel(UserQueries.GetById(id));
            //try
            //{
            //    UpdateModel(userModel, collection.ToValueProvider());
            //    userModel.Update(UserRepository);

            //    return RedirectToAction(UsersActionNames.Index);
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.Message);
            //    return View(userModel);
            //}
        }
    }
}