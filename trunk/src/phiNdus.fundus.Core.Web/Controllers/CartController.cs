using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.Models;
using phiNdus.fundus.Core.Web.State;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{    
    public class CartController : Controller
    {
        private static class Actions {
            public static string List { get { return @"List"; } }
        }

        public CartController() {
            this.StateManager = IoC.Resolve<IStateManager>();
        }

        private IStateManager StateManager { get; set; }

        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return RedirectToAction(Actions.List);
        }

        public ActionResult List() {
            return View(this.StateManager.Load<CartModel>());
        }

        public ActionResult Add() {
            var state = this.StateManager.Load<CartModel>();

            state.Items.Add(DateTime.Now.ToString());

            this.StateManager.Save(state);

            return RedirectToAction(Actions.List);
        }

        public ActionResult Clear() {
            this.StateManager.Remove<CartModel>();

            return RedirectToAction(Actions.List);
        }
    }
}
