namespace Phundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Articles.Commands;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Core.Inventory.Queries;
    using Models.ArticleModels;
    using phiNdus.fundus.Web.Helpers.FileUpload;
    using phiNdus.fundus.Web.ViewModels;

    public class ArticleController : ControllerBase
    {
        public IMemberInRole MemberInRole { get; set; }

        private static string MasterView
        {
            get { return @"_Tabs"; }
        }

        public IArticleQueries ArticleQueries { get; set; }

        public IImageQueries ImageQueries { get; set; }

        public IAvailabilityQueries AvailabilityQueries { get; set; }

        public IReservationRepository ReservationRepository { get; set; }

        [Transaction]
        public virtual ActionResult Create(Guid orgId)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleViewModel(orgId);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Create(Guid orgId, FormCollection collection)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleViewModel(orgId);
            try
            {
                UpdateModel(model, collection.ToValueProvider());

                var command = new CreateArticle
                {
                    Brand = model.Brand,
                    Color = model.Color,
                    Description = model.Description,
                    GrossStock = model.GrossStock,
                    InitiatorId = CurrentUserId,
                    Name = model.Name,
                    OwnerId = orgId,
                    Price = Convert.ToDecimal(model.Price),
                    Specification = model.Specification
                };
                Dispatcher.Dispatch(command);


                return RedirectToAction("Images", new {id = command.ArticleId});
            }

            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View("Create", model);
            }
        }

        [Transaction]
        public virtual ActionResult Edit(Guid orgId, int id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            return Fields(orgId, id);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Edit(Guid orgId, int id, FormCollection collection)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            return Fields(orgId, id, collection);
        }

        [Transaction]
        public virtual ActionResult Fields(Guid orgId, int id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleViewModel(
                ArticleQueries.GetById(id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Fields, model);
            }
            return View(Views.Fields, MasterView, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Fields(Guid orgId, int id, FormCollection collection)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleViewModel(orgId);
            try
            {
                UpdateModel(model, collection.ToValueProvider());

                Dispatcher.Dispatch(new UpdateArticle
                {
                    ArticleId = model.Id,
                    Brand = model.Brand,
                    Color = model.Color,
                    Description = model.Description,
                    GrossStock = model.GrossStock,
                    InitiatorId = CurrentUserId,
                    Name = model.Name,
                    Price = Convert.ToDecimal(model.Price),
                    Specification = model.Specification
                });

                return Redirect("/#/organizations/" + orgId + "/articles");
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // TODO: Exception-Handling
                ModelState.AddModelError("", ex.Message);
                return View(Views.Fields, model);
            }
        }

        [Transaction]
        public virtual ActionResult Images(Guid orgId, int id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var dto = ArticleQueries.GetById(id);
            if (dto == null)
                throw new HttpNotFoundException();

            var model = new ArticleViewModel(dto);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Images, model);
            return View(Views.Images, MasterView, model);
        }

        [AllowAnonymous]
        [Transaction]
        public virtual ActionResult ImageStore(Guid orgId, int id, string name)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var path = String.Format(@"~\Content\Images\Articles\{0}", id);
            var store = new ImageStore(path);

            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = Url.Content(path);
            factory.DeleteUrl = Url.Action("ImageStore", "Article");

            var handler = new BlueImpFileUploadHandler(store);

            if (Request.HttpMethod == "POST")
            {
                var images = handler.Post(HttpContext.Request.Files);
                foreach (var each in images)
                {
                    var command = new AddImage
                    {
                        ArticleId = id,
                        FileName = each.FileName,
                        InitiatorId = CurrentUserId,
                        Length = each.Length,
                        Type = each.Type
                    };
                    Dispatcher.Dispatch(command);
                    each.Id = command.ImageId.Value;
                }
                var result = factory.Create(images);
                return Json(result);
            }
            if (Request.HttpMethod == "GET")
            {
                var images = ImageQueries.ByArticle(id);
                var result = factory.Create(images);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Request.HttpMethod == "DELETE")
            {
                var fileName = store.FilePath + Path.DirectorySeparatorChar + name;

                Dispatcher.Dispatch(new RemoveImage
                {
                    ArticleId = id,
                    ImageFileName = fileName,
                    InitiatorId = CurrentUserId
                });
                store.Delete(name);
                return Json("");
            }
            return Json("");
        }

        [Transaction]
        public virtual ActionResult Availability(Guid orgId, int id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleAvailabilityViewModel();
            model.Id = id;
            model.Availabilites = AvailabilityQueries.GetAvailability(id).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Availability, model);
            }
            return View(Views.Availability, MasterView, model);
        }


        [Transaction]
        public virtual ActionResult Reservations(Guid orgId, int id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var model = new ArticleReservationsModel();
            model.Items = ReservationRepository.Find(id, Guid.Empty);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Reservations, model);
            return View(Views.Reservations, MasterView, model);
        }

        private static class Views
        {
            public static string Fields
            {
                get { return @"Fields"; }
            }

            public static string Images
            {
                get { return @"Images"; }
            }

            public static string Availability
            {
                get { return @"Availability"; }
            }

            public static string Reservations
            {
                get { return @"Reservations"; }
            }
        }
    }
}