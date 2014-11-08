namespace Phundus.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Core.Inventory.Queries;
    using Models.ArticleModels;
    using phiNdus.fundus.Web.Helpers.FileUpload;
    using phiNdus.fundus.Web.ViewModels;
    using Rest.Exceptions;

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
        public virtual ActionResult Index()
        {
            return RedirectToAction("list");
        }

        [Transaction]
        public virtual ActionResult List()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticlesTableViewModel(
                ArticleQueries.GetArticles(CurrentOrganizationId.Value)
                );
            return View(model);
        }

        [Transaction]
        public virtual ActionResult Create()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticleViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Create(FormCollection collection)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticleViewModel();
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
                    OrganizationId = CurrentOrganizationId.Value,
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
        public virtual ActionResult Edit(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            return Fields(id);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Edit(int id, FormCollection collection)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            return Fields(id, collection);
        }

        [Transaction]
        public virtual ActionResult Fields(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticleViewModel(
                ArticleQueries.GetArticle(id));
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Fields, model);
            }
            return View(Views.Fields, MasterView, model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Transaction]
        public virtual ActionResult Fields(int id, FormCollection collection)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticleViewModel();
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

                return RedirectToAction("Index");
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
        public virtual ActionResult Images(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var dto = ArticleQueries.GetArticle(id);
            if (dto == null)
                throw new HttpNotFoundException();

            var model = new ArticleViewModel(dto);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Images, model);
            return View(Views.Images, MasterView, model);
        }

        [AllowAnonymous]
        [Transaction]
        public virtual ActionResult ImageStore(int id, string name)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

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
        public virtual ActionResult Availability(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

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
        public virtual ActionResult Reservations(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new ArticleReservationsModel();
            model.Items = ReservationRepository.Find(id, Guid.Empty);
            if (Request.IsAjaxRequest())
                return PartialView(Views.Reservations, model);
            return View(Views.Reservations, MasterView, model);
        }

        [Transaction]
        public virtual ActionResult Categories(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var dto = ArticleQueries.GetArticle(id);
            if (dto == null)
                throw new HttpNotFoundException();

            var model = new ArticleViewModel(dto);
            if (Request.IsAjaxRequest())
            {
                return PartialView(Views.Categories, model);
            }
            return View(Views.Categories, MasterView, model);
        }

        [Transaction]
        public virtual ActionResult Delete(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var dto = ArticleQueries.GetArticle(id);
            if (dto == null)
                throw new HttpNotFoundException();

            return View(new ArticleViewModel(dto));
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Delete(int id, int version)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            Dispatcher.Dispatch(new DeleteArticle {ArticleId = id, InitiatorId = CurrentUserId});

            return RedirectToAction("Index");
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Transaction]
        public virtual ActionResult AjaxDelete(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            MessageBoxViewModel result;
            try
            {
                result = new MessageBoxViewModel
                {
                    Message = "Der Artikel wurde erfolgreich gelöscht.",
                    Type = MessageBoxType.Success
                };
            }
            catch (Exception ex)
            {
                result = new MessageBoxViewModel
                {
                    Message = ex.Message,
                    Type = MessageBoxType.Error
                };
            }
            return DisplayFor(result);
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

            public static string Categories
            {
                get { return @"Categories"; }
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