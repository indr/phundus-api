namespace phiNdus.fundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using phiNdus.fundus.Web.Helpers.FileUpload;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;
    using Phundus.Core.OrganisationCtx;
    using Phundus.Rest;
    using Phundus.Rest.Exceptions;

    [Authorize(Roles = "Chief")]
    public class FilesController : ControllerBase
    {
        public IUserRepository Users { get; set; }
        public IOrganizationRepository Organizations { get; set; }

        private string GetPath(int orgId)
        {
            return String.Format(@"~\Content\Uploads\Organizations\{0}", orgId);
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = Url.Content(path);
            factory.DeleteUrl = Url.Action("Delete", "Files");
            return factory;
        }

        //
        // GET: /Files/
        [HttpGet]
        [Transaction]
        public virtual JsonResult Index(int orgId)
        {
            var user = Users.FindByEmail(Identity.Name);
            var org = Organizations.FindById(orgId);
            if (!user.IsChiefOf(org))
                throw new HttpForbiddenException();

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(path);
            var files = store.GetFiles();
            return Json(factory.Create(files), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [Transaction]
        public virtual JsonResult Delete(int orgId, string id)
        {
            var user = Users.FindByEmail(Identity.Name);
            var org = Organizations.FindById(orgId);
            if (!user.IsChiefOf(org))
                throw new HttpForbiddenException();

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            store.Delete(id);
            return Json("");
        }

        [HttpPost]
        [Transaction]
        public virtual JsonResult Index(int orgId, string id)
        {
            var user = Users.FindByEmail(Identity.Name);
            var org = Organizations.FindById(orgId);
            if (!user.IsChiefOf(org))
                throw new HttpForbiddenException();

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(path);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Post(HttpContext.Request.Files);
            var result = factory.Create(images);
            return Json(result);
        }
    }
}